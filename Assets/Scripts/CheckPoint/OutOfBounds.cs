using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Player.playerTag))
        {
            CheckPointManager.Instance.StartRespawnSequence();
        }
    }
}
