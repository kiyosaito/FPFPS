using UnityEditor;
using UnityEngine;

public static class GroupCommand
{
    // Set meni item and add shortcut key Ctrl + G
    [MenuItem("GameObject/Group Selected %g")]
    private static void GroupSelected()
    {
        // Only create a group if we have transforms selected
        if (Selection.activeTransform)
        {
            // Create new empty GameObject to serve as the root of the group
            var gameObject = new GameObject(Selection.activeTransform.name + " Group");

            // Register undo action
            Undo.RegisterCreatedObjectUndo(gameObject, "Group Selected");

            // Parent the new root to the original parent of the selection
            gameObject.transform.SetParent(Selection.activeTransform.parent, false);

            // Parent everything selected to the new root object
            foreach (var transform in Selection.transforms)
            {
                Undo.SetTransformParent(transform, gameObject.transform, "Group Selected");
            }

            // Make the new root the active selection
            Selection.activeGameObject = gameObject;
        }
    }
}
