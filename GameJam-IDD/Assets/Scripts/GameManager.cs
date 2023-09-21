using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    private GameObject player;
    private GameObject[] lights;    
    [Header("Game Events")]
    public GameEvent OnGameStart;
    
    #region Player Information
    private Vector3 newPlayerStartPosition;
    #endregion
    
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        lights = GameObject.FindGameObjectsWithTag("Light");
    }
    private void Start()
    {
        GameStart();
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
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        HandlePlayerRestart();
        yield return new WaitForSeconds(0.5f);
        GetComponent<Transition>().FadeIn();
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