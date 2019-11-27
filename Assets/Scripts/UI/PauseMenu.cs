﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pause = null;

    [SerializeField]
    private GameObject pauseOptions = null;

    [SerializeField]
    private GameObject warnings = null;

    public bool ispaused;

    // TODO : For demo only, remove later
    [SerializeField]
    private bool isLevelEndMenu = false;

    private void Start()
    {
        ispaused = false;
        Unpausing();
    }
    private void Update()
    {

        if ((!isLevelEndMenu) && (InputManager.Instance.GetButtonDown(InputManager.InputKeys.Menu)))
        {
            if (ispaused == false)
            {
                Pausing();
            }
            else
            {
                Unpausing();
            }
        }
        
    }
    public void Pausing()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pause.gameObject.SetActive(true);
        ispaused = true;
    }
    public void Unpausing()
    {
        Time.timeScale = GameManager.Instance.GameTimeScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        warnings.gameObject.SetActive(false);
        pauseOptions.gameObject.SetActive(true);
        pause.gameObject.SetActive(false);
        ispaused = false;
    }
    public void QTM()
    {
        GameManager.Instance.BackToMain();
    }
    public void Quit()
    {
        Application.Quit();
    }

    // TODO : Remove this later, temporarily added for demo build, should be on level finished menu script
    public void NextLevel()
    {
        GameManager.Instance.LevelFinished();
    }
}
