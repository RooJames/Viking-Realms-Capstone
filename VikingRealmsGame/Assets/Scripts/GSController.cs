using UnityEngine;
using UnityEngine.UI;
public class GSController : MonoBehaviour
{
    [Header("Panel")]
    public GameObject settingsPanel;

    [Header("Audio")]
    public AudioSource gameMusic;
    public AudioSource uiAudioSource;
    public AudioClip clickSfx;
    public AudioClip scrollSfx;

    [Header("Settings - Slider")]
    public Slider masterVolumeSlider;

    [Header("Settings - Music Toggle")]
    public Image musicToggleImage;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

    [Header("Settings - SFX Toggle")]
    public Image sfxToggleImage;
    public Sprite sfxOnSprite;
    public Sprite sfxOffSprite;

    private bool settingsOpen = false;
    private bool musicOn = true;
    public static bool sfxOn = true;

    void Start()
    {
        Time.timeScale = 1f;
        if (settingsPanel) settingsPanel.SetActive(false);

        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.value = AudioListener.volume;
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        }

        if (gameMusic)
        {
            gameMusic.mute = !musicOn;
        }

        UpdateMusicUI();
        UpdateSFXUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (settingsOpen)
                CloseSettings();
            else
                OpenSettings();
        }
    }

    public void OpenSettings()
    {
        PlayScrollSFX();

        if (settingsPanel)
            settingsPanel.SetActive(true);

        settingsOpen = true;
        Time.timeScale = 0f;
    }

    public void CloseSettings()
    {
        PlayScrollSFX();

        if (settingsPanel)
            settingsPanel.SetActive(false);

        settingsOpen = false;
        Time.timeScale = 1f;
    }

    public void ToggleGameMusic()
    {
        PlayClick();

        musicOn = !musicOn;

        if (gameMusic)
            gameMusic.mute = !musicOn;

        UpdateMusicUI();
    }

    public void ToggleSFX()
    {
        PlayClick();

        sfxOn = !sfxOn;
        UpdateSFXUI();
    }

    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    private void UpdateMusicUI()
    {
        if (musicToggleImage != null)
            musicToggleImage.sprite = musicOn ? musicOnSprite : musicOffSprite;
    }

    private void UpdateSFXUI()
    {
        if (sfxToggleImage != null)
            sfxToggleImage.sprite = sfxOn ? sfxOnSprite : sfxOffSprite;
    }

    private void PlayClick()
    {
        if (sfxOn && uiAudioSource && clickSfx)
            uiAudioSource.PlayOneShot(clickSfx);
    }

    private void PlayScrollSFX()
    {
        if (sfxOn && uiAudioSource && scrollSfx)
            uiAudioSource.PlayOneShot(scrollSfx);
    }
}
