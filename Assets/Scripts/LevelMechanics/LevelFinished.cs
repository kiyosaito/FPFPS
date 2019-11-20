using UnityEngine;

public class LevelFinished : MonoBehaviour
{
    public void Trigger()
    {
        TimerManager.Instance.LevelFinished();
    }
}
