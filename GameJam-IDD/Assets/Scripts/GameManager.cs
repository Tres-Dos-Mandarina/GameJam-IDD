using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

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

    [Header("Audios")]
    private AudioSource src;
    public AudioClip mouseHover;
    public AudioClip mousePressed;
    public AudioClip deathAudio;

    private GameObject turboMainMenu;

    private List<Canvas> canvas;

    private void Awake()
    {
        roomLight = GameObject.Find("DoorLight") ? GameObject.Find("DoorLight") : null;
        door = GameObject.Find("Door") ? GameObject.Find("Door") : null;
        canvas = FindObjectsOfType<Canvas>().ToList();
        src = GetComponent<AudioSource>();
        // Eat cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        

        timer = GameObject.Find("Timer").GetComponent<Timer>();

        //
        //
        // IMPORTANT
        //
        // de la linia 70 a les 76 només shan de descomentar quan el joc es compili per web!!!!!!!!!!!!


        //foreach(Canvas c in canvas)
        //{
        //    c.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1280, 720);
        //}
        //GameObject.Find("Main Menu").GetComponent<Transform>().localScale = new Vector3(0.646440029f, 0.646440029f, 0.646440029f);
        //GameObject.Find("Main Menu").GetComponent<Transform>().position = new Vector3(768.0f, 393.0f, 0f);
        //GameObject.Find("Holder").GetComponent<Transform>().localScale = new Vector3(0.562837303f, 0.562837303f, 0.562837303f);

        menuCanvas = GameObject.Find("MenuCanvas");
        menuCanvas.SetActive(false);
        isMenuOn = false;
        mainMenu = GameObject.Find("Canvas");
        turboMainMenu = GameObject.Find("Main Menu");
    }
    public List<AudioSource> GetAllAudioSource()
    {
        List<AudioSource> allAudios = new List<AudioSource>();
        
        allAudios = FindObjectsOfType<AudioSource>().ToList();
        return allAudios;
    }
    public void MuteAllAudioSources(List<AudioSource> audios)
    {
        foreach (AudioSource audio in audios)
        {
            audio.enabled = false;
        }
    }
    public void TurnOnAllAudios(List<AudioSource> audios)
    {
        foreach (AudioSource audio in audios)
        {
            audio.enabled = true;
        }
    }
    private void Start()
    {
        player = GameObject.Find("Player");
        
        saveData = FindObjectOfType<SaveData>();
        if(!saveData.DoesFileExist())
        {
            saveData.SaveToJson(false, true, false);
        }
        else
        {
            saveData.LoadFromJson();
        }
        ;
        if (roomLight != null)
            roomLight.SetActive(false);
        
        if(door != null)
            door.SetActive(true);
        
        timer.gameObject.SetActive(saveData.config.speedrun);
        timer.ResumeTimer();
        GameStart();

        if(saveData.config.audio)
        {
            TurnOnAllAudios(GetAllAudioSource());
        }
        else
        {
            MuteAllAudioSources(GetAllAudioSource());
        }
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
        src.PlayOneShot(deathAudio);
        src.pitch = 1.8f;
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
        timer.StopTimer();
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
        if(SceneManager.GetActiveScene().name != "_MainMenu")
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
        else
        {
            Time.timeScale = 1f;
            menuCanvas.SetActive(false);
            mainMenu.SetActive(true);
            isMenuOn = false;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            turboMainMenu.SetActive(true);
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
        if(saveData.config.audio)
        {
            MuteAllAudioSources(GetAllAudioSource());
        }
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
                int scenes = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;

                if(SceneManager.GetActiveScene().buildIndex + 1 <= scenes -1)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    if (saveData.config.audio)
                    {
                        TurnOnAllAudios(GetAllAudioSource());
                    }
                }
                    
                else
                    SceneManager.LoadScene(0);
                canAdvanceLevel = false;
            }

        }        
    }
    public void HandleLightTurnOff(Component sender, object data)
    {
        timer.StartTimer();
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
    public void PlayMouseHoverSound()
    {
        src.clip = mouseHover;
        src.PlayOneShot(src.clip);
    }
    public void PlayMousePressedSound()
    {
        src.clip = mousePressed;
        src.PlayOneShot(src.clip);
    }
}