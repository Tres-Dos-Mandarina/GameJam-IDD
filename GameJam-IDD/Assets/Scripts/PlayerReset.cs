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
            gameObject.transform.position = spawnPoint.position;
        }
    }
}
