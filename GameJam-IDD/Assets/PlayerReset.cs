using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReset : MonoBehaviour
{
    private Transform spawnPoint;
    private Player player;

    private void Awake()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint").transform;
    }

    public void PlayerStartSet(Component sender, object data)
    {
        if (sender is GameManager)
        {
            this.gameObject.transform.localPosition = spawnPoint.position;

        }
    }
}
