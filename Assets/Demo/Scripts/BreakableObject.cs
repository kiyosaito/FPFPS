using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour, Target
{
    [SerializeField]
    private bool _chargedOnly = true;

    public void GetShot(bool charged, Vector3 point)
    {
        if (charged || !_chargedOnly)
        {
            GetDestroyed();
        }
        else
        {
            Resist();
        }
    }

    private void GetDestroyed()
    {
        // Do effect for being destroyed
        GetComponent<BoxCollider>().enabled = false;
        Destroy(gameObject);
    }

    private void Resist()
    {
        // Do effect indicating the attack failed
    }
}
