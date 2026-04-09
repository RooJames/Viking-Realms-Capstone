using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
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

    [Header("Settings - Text")]
    public TMP_Text mainMenuMusicText;

    [Header("Settings - Slider")]
    public Slider masterVolumeSlider;

    private bool musicOn = true;

    void Start()
    {
        ShowMainMenu();

        // Setup music
        if (mainMenuMusic)
        {
            mainMenuMusic.loop = true;
            mainMenuMusic.Play();
            mainMenuMusic.mute = !musicOn;
        }

        // Setup slider
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.value = AudioListener.volume;
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        }

        UpdateMusicText();
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

        UpdateMusicText();

        // FUTURE IMAGE TOGGLE (COMMENTED OUT)
        /*
        if (musicToggleImage != null)
        {
            musicToggleImage.sprite = musicOn ? musicOnSprite : musicOffSprite;
        }
        */
    }

    // ✅ MASTER VOLUME FUNCTION
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

    private void UpdateMusicText()
    {
        if (mainMenuMusicText)
        {
            mainMenuMusicText.text = musicOn
                ? "Music: On"
                : "Music: Off";
        }
    }

    private void PlayClick()
    {
        if (clickAudioSource && clickSfx)
            clickAudioSource.PlayOneShot(clickSfx);
    }

    // ============================================
    // 🔮 FUTURE IMAGE TOGGLE SYSTEM (NOT USED YET)
    // ============================================

    /*
    [Header("Future - Image Toggle")]
    public Image musicToggleImage;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    */
}