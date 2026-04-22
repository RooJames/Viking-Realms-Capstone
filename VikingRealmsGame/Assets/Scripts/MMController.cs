using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MMController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject titleStuffRoot;
    public GameObject menuPanel;
    public GameObject settingsPanel;

    [Header("Scene")]
    public string gameSceneName = "Game";

    [Header("Audio")]
    public AudioSource mainMenuMusic;
    public AudioSource clickAudioSource;
    public AudioClip clickSfx;

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

    private bool musicOn = true;
    private bool sfxOn = true;

    void Start()
    {
        ShowMainMenu();

        if (mainMenuMusic)
        {
            mainMenuMusic.loop = true;
            mainMenuMusic.Play();
            mainMenuMusic.mute = !musicOn;
        }

        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.value = AudioListener.volume;
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        }

        UpdateMusicUI();
        UpdateSFXUI();
    }

    public void OnPlayClicked()
    {
        PlayClick();
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnSettingsClicked()
    {
        PlayClick();

        if (titleStuffRoot) titleStuffRoot.SetActive(true);
        if (menuPanel) menuPanel.SetActive(false);
        if (settingsPanel) settingsPanel.SetActive(true);
    }

    public void OnQuitClicked()
    {
        PlayClick();
        Application.Quit();
    }

    public void OnBackFromSettings()
    {
        PlayClick();
        ShowMainMenu();
    }

    public void ToggleMainMenuMusic()
    {
        PlayClick();

        musicOn = !musicOn;

        if (mainMenuMusic)
        {
            mainMenuMusic.mute = !musicOn;
        }

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

    private void ShowMainMenu()
    {
        if (titleStuffRoot) titleStuffRoot.SetActive(true);
        if (menuPanel) menuPanel.SetActive(true);
        if (settingsPanel) settingsPanel.SetActive(false);
    }

    private void UpdateMusicUI()
    {
        if (musicToggleImage != null)
        {
            musicToggleImage.sprite = musicOn ? musicOnSprite : musicOffSprite;
        }
    }

    private void UpdateSFXUI()
    {
        if (sfxToggleImage != null)
        {
            sfxToggleImage.sprite = sfxOn ? sfxOnSprite : sfxOffSprite;
        }
    }

    private void PlayClick()
    {
        if (sfxOn && clickAudioSource && clickSfx)
        {
            clickAudioSource.PlayOneShot(clickSfx);
        }
    }
}