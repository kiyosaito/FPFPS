using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    private Player playerScript;
    private CheckPointManager checkPoint;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
        checkPoint = FindObjectOfType<CheckPointManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DisablePlayer();
            Invoke("RespawnPlayer",0.5f);
            Invoke("EnablePlayer", 0.5f);
        }
    }
    void DisablePlayer()
    {
        playerScript.enabled = false;
    }
    void RespawnPlayer()
    {
        player.transform.position = checkPoint.transform.position;
    }
    void EnablePlayer()
    {
        playerScript.enabled = true;
    }
}
