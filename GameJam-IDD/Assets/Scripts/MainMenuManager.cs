using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenuManager : MonoBehaviour
{
    private static MainMenuManager Instance { get; set; }

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    private VideoPlayer videoPlayer;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        videoPlayer = FindAnyObjectByType<VideoPlayer>();
    }

    private void Start()
    {
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "MainMenu.mp4");
        Debug.Log(videoPlayer.url);
        Debug.Log(System.IO.Path.Combine(Application.streamingAssetsPath, "MainMenu.mp4"));
        videoPlayer.Play();
        settingsMenu.SetActive(false);
    }

    public void LoadFirstLevel()
    {
        SceneManager.LoadScene("Level1");
    }

    public void OpenSettings()
    {
        Debug.Log("Abrir apartado de Settings");
        settingsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
