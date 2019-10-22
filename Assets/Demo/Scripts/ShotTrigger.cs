using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class ShotTrigger : MonoBehaviour,Target
{
    public string hitTag;
    public UnityEvent onEnter, onStay, onExit;
    public bool isActive = false;

    private void Reset()
    {
        Collider col = GetComponent<Collider>();
        if (col)
        {
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if((other.tag == hitTag || hitTag == "") && (isActive))
        {
            onEnter.Invoke();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if ((other.tag == hitTag || hitTag == "") && (isActive))
        {
            onStay.Invoke(); // Note (Manny): This was onEnter
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.tag == hitTag || hitTag == "") && (isActive))
        {
            onExit.Invoke(); // Note (Manny): This was also onEnter
        }
    }

    public void GetShot(bool charged)
    {
        isActive = true;

    }
}