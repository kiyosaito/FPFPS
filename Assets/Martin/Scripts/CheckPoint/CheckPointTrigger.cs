using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTrigger : CheckPointManager
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            checkpoint = gameObject.transform.position;
            gameObject.SetActive(false);
        }
    }
}
