using UnityEngine;
using UnityEngine.Events;

public class PlayerGroundedTrigger : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onGrounded = default;

    private void Update()
    {
        if (PlayerLink.Instance.PlayerInstance.IsGrounded)
        {
            onGrounded.Invoke();
        }
    }
}
