using UnityEngine;
using UnityEngine.SceneManagement;

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

    void Start()
    {
        ShowMainMenu();

        if (mainMenuMusic)
        {
            mainMenuMusic.loop = true;
            mainMenuMusic.Play();
        }
    }

    public void OnPlayClicked()
    {
        PlayClick();
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnSettingsClicked()
    {
        PlayClick();

        if (titleStuffRoot) titleStuffRoot.SetActive(false);
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

    private void ShowMainMenu()
    {
        if (titleStuffRoot) titleStuffRoot.SetActive(true);
        if (menuPanel) menuPanel.SetActive(true);
        if (settingsPanel) settingsPanel.SetActive(false);
    }

    private void PlayClick()
    {
        if (clickAudioSource && clickSfx)
            clickAudioSource.PlayOneShot(clickSfx);
    }
}