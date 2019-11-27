using System.Collections.Generic;
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

    private bool speedrunMode = false;

    private Vector3 checkpointLocation = Vector3.zero;
    private Quaternion checkpointRotation = Quaternion.identity;

    private List<ResetableObject> resetableObjects = null;

    private Dictionary<int, CheckPointTrigger> registeredCheckpoints = new Dictionary<int, CheckPointTrigger>();

    #endregion

    #region Public Properties

    public int CheckpointCount
    {
        get { return registeredCheckpoints.Count; }
    }

    #endregion

    #region MonoBehaviour Functions

    private void Update()
    {
        if (Application.isEditor)
        {
            if (Input.anyKeyDown)
            {
                KeyCode[] numbers = { KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, };

                for (int i = 0; i < numbers.Length; ++i)
                {
                    if (Input.GetKeyDown(numbers[i]) && registeredCheckpoints.ContainsKey(i))
                    {
                        TimerManager.Instance.InvalidateTimes();
                        CheckpointReached(i);
                        StartRespawnSequence();
                        break;
                    }
                }
            }
        }
    }

    #endregion

    #region Public Functions

    public void Init()
    {
        speedrunMode = GameManager.Instance.SpeedrunMode;
        foreach (var checkpoint in FindObjectsOfType<CheckPointTrigger>())
        {
            checkpoint.Register();
        }
    }

    public void CheckpointReached(int checkpointID)
    {
        checkpointLocation = registeredCheckpoints[checkpointID].SpawnLocation.position;
        checkpointRotation = registeredCheckpoints[checkpointID].SpawnLocation.rotation;

        SaveStates();

        TimerManager.Instance.CheckpointReached(checkpointID);
    }

    public void StartRespawnSequence()
    {
        DisablePlayer();
    }

    public void RegisterCheckpoint(int checkpointID, CheckPointTrigger checkpoint)
    {
        if (!registeredCheckpoints.ContainsKey(checkpointID))
        {
            registeredCheckpoints.Add(checkpointID, checkpoint);
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
        // TODO: Add visual effects
        PlayerLink.Instance.PlayerInstance.enabled = false;
        Invoke("RespawnPlayer", 0.5f);
    }

    private void RespawnPlayer()
    {
        if ((speedrunMode) && (GameManager.GameScene.Unknown != GameManager.Instance.GetCurrentScene()))
        {
            GameManager.Instance.RestartLevel();
        }
        else
        {
            // TODO: Add visual effects
            PlayerLink.Instance.PlayerInstance.Warp(checkpointLocation);
            PlayerLink.Instance.PlayerInstance.ResetPlayer();
            PlayerLink.Instance.CameraLookInstance.ResetCamera(checkpointRotation);
            PlayerLink.Instance.WeaponInstance.ResetWeapon();
            ResetStates();
            Invoke("EnablePlayer", 0.5f);
        }
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
