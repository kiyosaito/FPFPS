using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFollow : MonoBehaviour
{
    [SerializeField]
    private Transform _followTarget = null;

    private void Update()
    {
        if (null != _followTarget)
        {
            transform.rotation = _followTarget.rotation;
        }
    }
}
