using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    #region Player Events
        public GameEvent OnPlayerDeath;
        public GameEvent OnPlayerNewLevel;
    #endregion

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
        if (collision.gameObject.CompareTag("Death"))
        {
            Debug.Log("Player died (so sad)");
            OnPlayerDeath.Raise(this, collision.gameObject.name);
        }
    }
}
