using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtDirection : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void LateUpdate()
    {
        //
        transform.rotation = Quaternion.LookRotation(target.forward);
    }
}
