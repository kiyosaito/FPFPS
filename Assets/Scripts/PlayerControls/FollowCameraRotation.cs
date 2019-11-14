using UnityEngine;

public class FollowCameraRotation : MonoBehaviour
{
    [SerializeField]
    private Camera _cam = null;

    private void Start()
    {
        if (null == _cam)
        {
            _cam = Camera.main;
        }
    }

    private void LateUpdate()
    {
        transform.rotation = _cam.transform.rotation;
    }
}
