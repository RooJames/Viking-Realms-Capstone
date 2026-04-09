using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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

    [Header("Settings")]
    public TMP_Text mainMenuMusicText;

    private bool musicOn = true;

    void Start()
    {
        ShowMainMenu();

        if (mainMenuMusic)
        {
            mainMenuMusic.loop = true;
            mainMenuMusic.Play();
            mainMenuMusic.mute = !musicOn;
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
                ? "Music:On"
                : "Music:Off";
        }
    }

    private void PlayClick()
    {
        if (clickAudioSource && clickSfx)
            clickAudioSource.PlayOneShot(clickSfx);
    }
}