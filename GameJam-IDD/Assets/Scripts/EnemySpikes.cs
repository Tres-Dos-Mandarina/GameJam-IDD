using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpikes : MonoBehaviour
{
    [Header("Game Events")] 
    public GameEvent onPlayerDeath;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            onPlayerDeath.Raise(this, null);
        }
    }
}
