using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{
    #region Private Variables

    [SerializeField]
    private int checkpointID = -1;

    // In case the player somehow manages to skip a checkpoint, we'd want to disable previous checkpints, to avoid the player accidentally making backwards progress
    [SerializeField]
    private CheckPointTrigger previousCheckpoint = null;

    // In case the center of the checkpoint trigger area is not suitable as a respawn location, a transform can be added to specify the exact position
    [SerializeField]
    private Transform spawnLocation = null;

    #endregion

    #region Public Properties

    public Transform SpawnLocation
    {
        get
        {
            Transform spawnloc = null;

            if (null != spawnLocation)
            {
                spawnloc = spawnLocation;
            }
            else
            {
                spawnloc = transform;
            }

            return spawnloc;
        }
    }

    #endregion

    #region MonoBehaviour Functions

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Player.playerTag))
        {
            CheckPointManager.Instance.CheckpointReached(checkpointID);
            DisableCheckpoint();
        }
    }

    #endregion

    #region Public Functions

    public void Register()
    {
        if (-1 != checkpointID)
        {
            CheckPointManager.Instance.RegisterCheckpoint(checkpointID, this);
        }
        else
        {
            Debug.LogError("Checkpoint ID not set");
        }
    }

    #endregion

    #region Private Functions

    private void DisableCheckpoint()
    {
        if (null != previousCheckpoint)
        {
            previousCheckpoint.DisableCheckpoint();
        }

        gameObject.SetActive(false);
    }

    #endregion
}
