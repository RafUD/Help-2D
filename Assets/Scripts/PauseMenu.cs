using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class PauseMenu : MonoBehaviour 
{

    public static bool Paused = false;
    public GameObject PauseMenuCanvas;

    void Start()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Paused)
            {
                Play();
            }
            else 
            {
                Stop();
            }

        }
    }

    public void Stop()
    {
        PauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        Paused = true;
    }

    public void Play()
    {
        PauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        Paused = false;
    }

    //To go back to menu. 'public' so can use OnClickEvents
    public void MainMenuButton()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
