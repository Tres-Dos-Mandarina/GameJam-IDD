using UnityEngine;

public class EnemyReset : MonoBehaviour
{

    Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    public void EnemyStartSet(Component sender, object data)
    {
        enemy.SetEnemyPosition(enemy.enemyPosSave);
        enemy.SetEnemyState(enemy.enemyStateSave);
        enemy.SetEnemyDirection(enemy.enemyDirectionSave);
        enemy.SetMovementSpeed(enemy.movementSpeedSave);
    }
}