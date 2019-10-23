using UnityEngine;
using UnityEngine.Events;

public class ShotTrigger : MonoBehaviour, Target
{
    [SerializeField]
    private UnityEvent onNormalShoot = default;

    [SerializeField]
    private UnityEvent onChargedShoot = default;

    [SerializeField]
    private UnityEvent onShoot = default;

    public void GetShot(bool charged, Vector3 point)
    {
        onShoot.Invoke();

        if (charged)
        {
            onChargedShoot.Invoke();
        }
        else
        {
            onNormalShoot.Invoke();
        }
    }
}