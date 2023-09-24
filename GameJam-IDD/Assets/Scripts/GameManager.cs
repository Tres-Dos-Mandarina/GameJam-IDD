using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    private GameObject player;
    public GameObject[] lights;

    // Menus
    private GameObject menuCanvas;
    private bool isMenuOn = false;

    // Lights
    private GameObject roomLight;
    private GameObject door;
    
    [Header("Game Events")]
    public GameEvent OnGameStart;
    private bool nextLevel;
    public bool isAnyKeyPress = false;
    
    private bool canAdvanceLevel = true;
    private Vector3 newPlayerStartPosition;
    private SaveData saveData;

    private Timer timer;

    private GameObject mainMenu;

    private void Awake()
    {
        roomLight = GameObject.Find("DoorLight");
        door = GameObject.Find("Door");
        // Eat cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);

        saveData = FindObjectOfType<SaveData>();
        if(!saveData.DoesFileExist())
        {
            saveData.SaveToJson(false, true, false);
        }
        else
        {
            saveData.LoadFromJson();
        }
        menuCanvas = GameObject.Find("MenuCanvas");
        menuCanvas.SetActive(false);
        isMenuOn = false;

        timer = GameObject.Find("Timer").GetComponent<Timer>();

        mainMenu = GameObject.Find("Main Menu");
    }
    private void Start()
    {
        player = GameObject.Find("Player");
        roomLight.SetActive(false);
        timer.gameObject.SetActive(saveData.config.speedrun);
        door.SetActive(true);
        GameStart();
    }
    public void ChangeLevel()
    {
        nextLevel = true;
    }
    public void GameStart()
    {
        OnGameStart.Raise(this, "GameStart Event On air");
    }

    public void HandlePlayerDeath(Component sender, object data)
    {

        StartCoroutine(LoadFastLevel());

        //HandlePlayerRestart();
    }
    public IEnumerator LoadFastLevel()
    {
        player.SetActive(false);
        yield return new WaitForSeconds(.25f);
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void HandlePlayerRestart()
    {
        GameStart();
    }
    
    public void HandlePlayerGoal(Component sender, object data)
    {
        player.GetComponent<Player>().DisableMovement();
        
        HandleNextLevel();
    } 

    public void GoToMainMenu()
    {
        BackToGameFromSettings();
        SceneManager.LoadScene("_MainMenu");
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if(isMenuOn)
            {

                BackToGameFromSettings();
            }
            else
            {
                OpenSettingsFromGame();
            }
        }
    }

    public void BackToGameFromSettings()
    {
        Time.timeScale = 1f;
        menuCanvas.SetActive(false);
        saveData.SaveToJson(saveData.config.speedrun, saveData.config.audio, saveData.config.kenkri);
        timer.gameObject.SetActive(saveData.config.speedrun);
        timer.ResumeTimer();
        isMenuOn = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (mainMenu != null)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            mainMenu.SetActive(true);
        }
    }

    public void OpenSettingsFromGame()
    {
        Time.timeScale = 0f;
        timer.StopTimer();
        menuCanvas.SetActive(true);
        isMenuOn = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void HandleNextLevel()
    {
        StartCoroutine(HandleLoadScene());
    }
    
    IEnumerator HandleLoadScene()
    {
        GetComponent<Transition>().FadeOut();
        yield return new WaitForSeconds(2);
        while (!nextLevel)
        {
            yield return null;

            if (Input.anyKey)
            {
                isAnyKeyPress = true;
            }
            else
            {
                if (isAnyKeyPress)
                {
                    canAdvanceLevel = true;
                    isAnyKeyPress = false;
                }
            }

            if (canAdvanceLevel && Input.anyKeyDown)
            {
                Debug.Log("Avanzando al siguiente nivel");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                canAdvanceLevel = false;
            }
        }        
    }
    public void HandleLightTurnOff(Component sender, object data)
    { 
        foreach(var ligh in lights)
        {
            if(ligh.tag != "WorldLight")
            {
                ligh.gameObject.SetActive(false);
                roomLight.SetActive(true);
                door.SetActive(false);
            }
            else
            {
                ligh.gameObject.SetActive(true);
            }
        }
    }
}