using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DisablePlayer();
            Invoke("RespawnPlayer",0.5f);
            Invoke("EnablePlayer", 1.5f);
        }
    }
    void DisablePlayer()
    {
        PlayerLink.Instance.PlayerInstance.enabled = false;
    }
    void RespawnPlayer()
    {
        PlayerLink.Instance.PlayerInstance.gameObject.transform.position = CheckPointManager.Instance.checkpoint;
    }
    void EnablePlayer()
    {
        PlayerLink.Instance.PlayerInstance.enabled = true;
    }
}
