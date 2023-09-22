using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private static MainMenuManager Instance { get; set; }

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;

    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
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
}
