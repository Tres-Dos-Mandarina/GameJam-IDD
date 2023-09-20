using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    private Player player;
    #region Player Events
        public GameEvent OnPlayerDeath;
        public GameEvent OnPlayerNewLevel;
    #endregion

    public GameEvent _onLand;
    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void Start()
    {
        PlayerEnteredNewLevel();
    }

    private void PlayerEnteredNewLevel()
    {
        OnPlayerNewLevel.Raise(this, transform.position);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _onLand.Raise(this, " ");
        
        if (collision.gameObject.CompareTag("Death"))
        {
            Debug.Log("Player died (so sad)");
            OnPlayerDeath.Raise(this, collision.gameObject.name);
        }
    }
}
