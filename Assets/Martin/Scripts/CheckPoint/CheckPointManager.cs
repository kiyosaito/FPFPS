﻿using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : UnitySingleton<CheckPointManager>
{
    #region Function Overrides

    protected override bool ShouldSurviveSceneChange() { return false; }

    protected override void Setup()
    {
        resetableObjects = new List<ResetableObject>(FindObjectsOfType<ResetableObject>());

        enabled = Application.isEditor;
    }

    #endregion

    #region Private Variables

    private Vector3 checkpointLocation = Vector3.zero;
    private Quaternion checkpointRotation = Quaternion.identity;

    private List<ResetableObject> resetableObjects = null;

    private Dictionary<int, Transform> registeredCheckpoints = new Dictionary<int, Transform>();

    #endregion

    #region MonoBehaviour Functions

    private void Update()
    {
        if (Application.isEditor)
        {
            KeyCode[] numbers = { KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, };

            for (int i = 0; i < numbers.Length; ++i)
            {
                if (Input.GetKeyDown(numbers[i]) && registeredCheckpoints.ContainsKey(i))
                {
                    CheckpointReached(registeredCheckpoints[i]);
                    StartRespawnSequence();
                    break;
                }
            }
        }
    }

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

    public void RegisterSpawnLocation(int checkpointID, Transform spawnLocation)
    {
        if (!registeredCheckpoints.ContainsKey(checkpointID))
        {
            registeredCheckpoints.Add(checkpointID, spawnLocation);
        }
        else
        {
            Debug.LogError("Two checkpoints registered with ID " + checkpointID.ToString());
        }
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
