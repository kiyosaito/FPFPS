using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebindMenu : MonoBehaviour
{
    #region Private Variables

    // List to reference rebind buttons quickly
    private List<RebindButton> _buttons = null;

    #endregion

    #region MonoBehavious Functions

    private void Start()
    {
        // Initialise rebind button list
        _buttons = new List<RebindButton>(GetComponentsInChildren<RebindButton>());

        // Check for duplicate keybinds
        CheckForDuplicates();
    }

    #endregion

    #region Public Functions

    public void CheckForDuplicates()
    {
        // Go through all buttons in the list
        for (int i = 0; i < _buttons.Count; ++i)
        {
            // Set then as not duplicates
            _buttons[i].SetAsDuplicate(false);

            // Go through all buttons in the list after the current one
            for (int j = i + 1; j < _buttons.Count; ++j)
            {
                // If they have the same keybind (that isn't None), then mark both as duplicates
                if ((KeyCode.None != _buttons[i].MappedKey) && (_buttons[i].MappedKey == _buttons[j].MappedKey))
                {
                    _buttons[i].SetAsDuplicate(true);
                    _buttons[j].SetAsDuplicate(true);

                    // We don't break, as there could be more duplicates later in the list, so we have to go through all
                }
            }
        }
    }

    #endregion
}
