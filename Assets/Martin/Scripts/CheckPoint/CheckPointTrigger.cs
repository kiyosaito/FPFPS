using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{
    #region Private Variables

    // In case the player somehow manages to skip a checkpoint, we'd want to disable previous checkpints, to avoid the player accidentally making backwards progress
    [SerializeField]
    private CheckPointTrigger previousCheckpoint = null;

    // In case the center of the checkpoint trigger area is not suitable as a respawn location, a transform can be added to specify the exact position
    [SerializeField]
    private Transform spawnLocation = null;

    private Transform SpawnLocation
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
            CheckPointManager.Instance.CheckpointReached(SpawnLocation);
            DisableCheckpoint();
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
