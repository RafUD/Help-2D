using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [Header("----------Audio Sources----------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("----------Audio Clips (SFX)----------")]
    public AudioClip jumpSFX;
    public AudioClip shootSFX;
    public AudioClip empoweredShootSFX;
    public AudioClip enemyImpactSFX;
    public AudioClip playerImpackSFX;
    public AudioClip deathEnemySFX;
    public AudioClip powerUpSFX;
    public AudioClip healSFX;
    public AudioClip playerMovementSFX;
    public AudioClip confirmUISFX;
    public AudioClip cancelUISFX;
    public AudioClip pauseSFX;
    public AudioClip unpauseSFX;
    public AudioClip hoverSFX;
    public AudioClip coinSFX;
    public AudioClip fallSFX;

    [Header("----------Music Clips----------")]
    public AudioClip niveauRaf;
    public AudioClip menuMusic;
    public AudioClip niveauAhmed;
    public AudioClip gameWonMusic;
    public AudioClip gameOverMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.activeSceneChanged += OnSceneChanged; // listen for scene changes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnDestroy()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged; // cleanup
    }

    private void Start()
    {
        // Play correct music for initial scene
        UpdateMusicForScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        UpdateMusicForScene(newScene.name);
    }

    private void UpdateMusicForScene(string sceneName)
    {
        if (musicSource == null) return;

        if (sceneName.Contains("Menu"))
            PlayMusic(menuMusic);
        else if (sceneName.Contains("Raf"))
            PlayMusic(niveauRaf);
        else if (sceneName.Contains("Ahmed"))
            PlayMusic(niveauAhmed);
        else if (sceneName.Contains("Won"))
            PlayMusic(gameWonMusic);
    }

    // Play one-shot sound effects
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null && sfxSource != null)
            sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;

        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayGameOverMusic()
    {
        if (musicSource == null || gameOverMusic == null) return;

        musicSource.Stop();
        musicSource.clip = gameOverMusic;
        musicSource.loop = false;
        musicSource.Play();
    }

    public void PlayGameWonMusic()
    {
        if (musicSource == null || gameWonMusic == null) return;

        musicSource.Stop();
        musicSource.clip = gameWonMusic;
        musicSource.loop = false;
        musicSource.Play();
    }
}
