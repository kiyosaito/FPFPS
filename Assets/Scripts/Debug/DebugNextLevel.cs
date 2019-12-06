using UnityEngine;

public class DebugNextLevel : MonoBehaviour
{
    public void Trigger()
    {
        GameManager.Instance.LevelFinished();
        GameManager.Instance.LoadNextLevel();
    }
}
