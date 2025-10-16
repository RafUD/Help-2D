using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool Paused = false;
    public GameObject PauseMenuCanvas;

    private AudioManager audioManager;

    void Start()
    {
        Paused = false;
        Time.timeScale = 1f;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Paused)
                Play();
            else
            Stop();
        }
    }

    public void Stop()
    {
        PauseMenuCanvas.SetActive(true);
        Debug.Log("Pause menu canvas!!");
        Time.timeScale = 0f;
        Debug.Log("Pause: time frozen!!");
        Paused = true;

        if (audioManager != null && audioManager.pauseSFX != null)
            audioManager.PlaySFX(audioManager.pauseSFX);
    }

    public void Play()
    {
        PauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        Paused = false;

        if (audioManager != null && audioManager.unpauseSFX != null)
            audioManager.PlaySFX(audioManager.unpauseSFX);
    }

    public void MainMenuButton()
    {
        if (audioManager != null && audioManager.cancelUISFX != null)
            audioManager.PlaySFX(audioManager.cancelUISFX);

        SceneManager.LoadScene("Main Menu");
    }
}
