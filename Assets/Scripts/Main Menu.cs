using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private AudioManager audioManager;

    void Start()
    {
        // Just find the AudioManager � no need to manually start music
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
    }

    public void Play()
    {
        if (audioManager != null)
            audioManager.PlaySFX(audioManager.confirmUISFX);

        SceneManager.LoadScene("NiveauRaf");
    }

    public void Restart()
    {
        if (audioManager != null)
            audioManager.PlaySFX(audioManager.confirmUISFX);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        if (audioManager != null)
            audioManager.PlaySFX(audioManager.cancelUISFX);

        Application.Quit();
        Debug.Log("Quit game");
    }

    // Optional: for UI hover sound
    public void HoverSound()
    {
        if (audioManager != null)
            audioManager.PlaySFX(audioManager.hoverSFX, 0.3f);
    }
}
