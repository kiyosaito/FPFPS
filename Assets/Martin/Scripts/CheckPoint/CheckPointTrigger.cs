using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CheckPointManager.Instance.checkpoint = gameObject.transform.position;
            gameObject.SetActive(false);
        }
    }
}
