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

        menuCanvas = GameObject.Find("MenuCanvas");
        menuCanvas.SetActive(false);
        isMenuOn = false;
    }
    private void Start()
    {
        player = GameObject.Find("Player");
        roomLight.SetActive(false);
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
        HandlePlayerRestart();
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
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if(isMenuOn)
            {
                menuCanvas.SetActive(false);
                isMenuOn = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                menuCanvas.SetActive(true);
                isMenuOn = true;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
        }
    }
    public void HandleNextLevel()
    {
        StartCoroutine(HandleLoadScene());
    }
    
    IEnumerator HandleLoadScene()
    {
        // TODO Stop Player movement
        GetComponent<Transition>().FadeOut();
        yield return new WaitForSeconds(2);
        while (!nextLevel)
        {
            yield return null;

            if (Input.anyKey)
            {
                // Si se está presionando una tecla, marca que hay una tecla presionada
                isAnyKeyPress = true;
            }
            else
            {
                // Si no se está presionando ninguna tecla, se permite avanzar de nivel si había una tecla presionada previamente
                if (isAnyKeyPress)
                {
                    canAdvanceLevel = true;
                    isAnyKeyPress = false; // Reiniciar el estado de tecla presionada
                }
            }

            // Verificar si se debe avanzar de nivel
            if (canAdvanceLevel && Input.anyKeyDown) // Detecta cualquier tecla presionada, incluyendo las que ya estén siendo presionadas
            {
                // Aquí puedes poner el código para avanzar de nivel
                // Por ejemplo, cargar el siguiente nivel o realizar alguna acción relacionada con el avance de nivel
                Debug.Log("Avanzando al siguiente nivel");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                
                // Deshabilita la posibilidad de avanzar nuevamente hasta que se levante y vuelva a presionar una tecla
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