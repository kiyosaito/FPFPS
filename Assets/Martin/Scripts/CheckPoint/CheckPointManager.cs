using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : UnitySingleton<CheckPointManager>
{
    #region Function Overrides

    protected override bool ShouldSurviveSceneChange() { return false; }

    protected override void Setup()
    {
        resetableObjects = new List<ResetableObject>(FindObjectsOfType<ResetableObject>());
    }

    #endregion

    #region Private Variables

    private Vector3 checkpointLocation = Vector3.zero;
    private Quaternion checkpointRotation = Quaternion.identity;

    private List<ResetableObject> resetableObjects = null;

    #endregion

    #region Public Functions

    public void CheckpointReached(Transform respawnTransform)
    {
        checkpointLocation = respawnTransform.position;
        checkpointRotation = respawnTransform.rotation;

        SaveStates();
    }

    public void StartRespawnSequence()
    {
        DisablePlayer();
    }

    #endregion

    #region Private Functions

    private void DisablePlayer()
    {
        PlayerLink.Instance.PlayerInstance.enabled = false;
        Invoke("RespawnPlayer", 0.5f);
    }

    private void RespawnPlayer()
    {
        // TODO: Add visual effect to hide reset
        PlayerLink.Instance.PlayerInstance.Warp(checkpointLocation);
        PlayerLink.Instance.PlayerInstance.ResetPlayer();
        PlayerLink.Instance.CameraLookInstance.ResetCamera(checkpointRotation);
        PlayerLink.Instance.WeaponInstance.ResetWeapon();
        ResetStates();
        Invoke("EnablePlayer", 0.5f);
    }

    private void EnablePlayer()
    {
        PlayerLink.Instance.PlayerInstance.enabled = true;
    }

    private void SaveStates()
    {
        if (null != resetableObjects)
        {
            foreach (var resetableObject in resetableObjects)
            {
                resetableObject.SaveState();
            }
        }
    }

    private void ResetStates()
    {
        if (null != resetableObjects)
        {
            foreach (var resetableObject in resetableObjects)
            {
                resetableObject.ResetState();
            }
        }
    }

    #endregion
}
