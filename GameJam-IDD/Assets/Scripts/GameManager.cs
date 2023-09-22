using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    private GameObject player;
    private GameObject[] lights;
    
    [Header("Game Events")]
    public GameEvent OnGameStart;
    private bool nextLevel;
    public bool isAnyKeyPress = false;
    
    private bool canAdvanceLevel = true;
    private Vector3 newPlayerStartPosition;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);

        lights = GameObject.FindGameObjectsWithTag("Light");
    }
    private void Start()
    {
        player = GameObject.Find("Player");
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
        Debug.Log("Player Died");
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
        Debug.Log("Turning Off Lights");
        foreach(var light in lights)
        {
            light.gameObject.SetActive(false);
        }
    }
}