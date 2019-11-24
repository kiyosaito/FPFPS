using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private GameObject pause;
    private GameObject pauseOptions;
    private GameObject warnings;
    public bool ispaused;

    private void Start()
    {
        pause = GameObject.Find("Hologram");
        pauseOptions = GameObject.Find("PauseOptions");
        warnings = GameObject.Find("Warnings");
        ispaused = false;
        Unpausing();
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
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
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pause.gameObject.SetActive(true);
        ispaused = true;
    }
    public void Unpausing()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        warnings.gameObject.SetActive(false);
        pauseOptions.gameObject.SetActive(true);
        pause.gameObject.SetActive(false);
        ispaused = false;
    }
    public void QTM()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
