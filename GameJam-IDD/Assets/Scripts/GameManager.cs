using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    private GameObject player;
    
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
        Debug.Log("Handling player goal");
        HandleNextLevel();
    } 

    public void HandleNextLevel()
    {
        StartCoroutine(HandleTransition());
    }

    IEnumerator HandleTransition()
    {
        // TODO Stop Player movement
        GetComponent<Transition>().FadeOut();
        yield return new WaitForSeconds(2f);
        HandlePlayerRestart();
        GetComponent<Transition>().FadeIn();
    }
}