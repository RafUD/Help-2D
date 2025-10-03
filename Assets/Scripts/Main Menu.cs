using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void Play()
    {
        SceneManager.LoadScene("NiveauRaf");
    }

    //public void Instructions()
    //{
    //    SceneManager.LoadScene("Instructions");
    //}


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit game");
    }

}
