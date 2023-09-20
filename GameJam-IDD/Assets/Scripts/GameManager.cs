using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    private GameObject player;
    #region GameEvents
        public GameEvent OnPlayerStart;
    #endregion

    #region Player Information
    private Vector3 newPlayerStartPosition;
    #endregion
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        player = GameObject.Find("Player");
    }
    private void Start()
    {
        GameStart();
    }
    public void GameStart()
    {
        OnPlayerStart.Raise(this, "DoInitialize");
    }
        
    public void GoalHandler(Component sender, object data)
    {
        if (sender is Goal)
        {
            
        }
    }   

    public void HandlePlayerDeath(Component sender, object data)
    {
        Debug.Log("Player died because of: " + (string)data);
        DoPlayerRestart();
    }
    public void HandlePlayerNewLevel(Component sender, object data)
    {
        Debug.Log((Vector3?)data);
        if(data is Vector3)
            newPlayerStartPosition = (Vector3)data;
    }

    public void DoPlayerRestart()
    {
        Debug.Log("This is where player should spawn now: " + newPlayerStartPosition);
        player.transform.position = newPlayerStartPosition;
    }
}