using System;
using UnityEngine;

public enum EnemyState
{
    IDLE,
    MOVING
}

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyState _enemyState;

    private Rigidbody2D rb;

    public float movementSpeed;
    private Vector3 startPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        startPos = transform.position;
    }

    void Start()
    {
        _enemyState = EnemyState.IDLE;
    }

    void Update()
    {
        if (Input.GetButtonDown("Interaction") && _enemyState == EnemyState.IDLE)
        {
            _enemyState = EnemyState.MOVING;
        }

        if (_enemyState == EnemyState.MOVING)
        {
            rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Debug.Log("Player Hit!");
            _enemyState = EnemyState.IDLE;
            rb.velocity = new Vector2(0, 0);
            transform.position = startPos;
        }
    }
}
