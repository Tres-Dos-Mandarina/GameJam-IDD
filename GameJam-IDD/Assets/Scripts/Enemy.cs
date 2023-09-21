using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum EnemyState
{
    Idle,
    Moving
}

public enum EnemyDirection
{
    Right,
    Left,
    Up,
    Down
}

public class Enemy : MonoBehaviour
{
    private Rigidbody2D _rb;
    private readonly Quaternion _leftRot = new Quaternion(0f, 1f, 0f, 0f);
    private readonly Quaternion _upRot = new Quaternion(0f, 0f, 0.707f, 0.707f);
    private readonly Quaternion _downRot = new Quaternion(0f, 0f, -0.707f, 0.707f);
    
    [Header("Basic Configuration of the Enemy")]
    public EnemyState enemyState = EnemyState.Idle;
    public EnemyDirection enemyDirection;
    public float movementSpeed;
    [HideInInspector] public Vector3 enemyPosSave;
    [HideInInspector] public EnemyState enemyStateSave;
    [HideInInspector] public EnemyDirection enemyDirectionSave;
    [HideInInspector] public float movementSpeedSave;

    [Header("Game Events")] 
    public GameEvent onPlayerDeath;
    public GameEvent onLightTurnOff;

    #region LifeCycle

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

        // Save Enemy Config
        enemyPosSave = transform.position;
        enemyStateSave = enemyState;
        enemyDirectionSave = enemyDirection;
        movementSpeedSave = movementSpeed;
    }

    void Update()
    {
        if (Input.GetButtonDown("Interaction") && enemyState == EnemyState.Idle)
        {
            Debug.Log("Interaction");
            onLightTurnOff.Raise(this, null);
            SetEnemyState(EnemyState.Moving);
        }
    }

    private void FixedUpdate()
    {
        if (enemyState != EnemyState.Moving)
        {
            _rb.velocity = new Vector2(0f, 0f);
            return;
        }
        switch (enemyDirection)
        {
            case EnemyDirection.Right:
                ApplyMovement(movementSpeed, 0f);
                break;
            case EnemyDirection.Left:
                ApplyMovement(-movementSpeed, 0f);
                break;
            case EnemyDirection.Up:
                ApplyMovement(0f, movementSpeed);
                break;
            case EnemyDirection.Down:
                ApplyMovement(0f, -movementSpeed);
                break;
        }
    }

    #endregion
    
    #region Setters

    public void SetEnemyPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public void SetEnemyState(EnemyState newState)
    {
        enemyState = newState;
    }

    public void SetEnemyDirection(EnemyDirection newDirection)
    {
        enemyDirection = newDirection;
        switch (enemyDirection)
        {
            case EnemyDirection.Right:
                transform.rotation = Quaternion.identity;
                break;
            case EnemyDirection.Left:
                transform.rotation = _leftRot;
                break;
            case EnemyDirection.Up:
                transform.rotation = _upRot;
                break;
            case EnemyDirection.Down:
                transform.rotation = _downRot;
                break;
        }
    }

    public void SetMovementSpeed(float newSpeed)
    {
        movementSpeed = newSpeed;
    }
    
    private void ApplyMovement(float x, float y)
    {
        _rb.velocity = new Vector2(x, y);
    }

    #endregion
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            onPlayerDeath.Raise(this, null);
            SetEnemyState(EnemyState.Idle);
        }
    }
}
