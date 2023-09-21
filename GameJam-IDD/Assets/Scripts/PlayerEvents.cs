using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rb;
    private FeedbacksManager feedbacksManager;
    #region Player Events
        public GameEvent OnPlayerDeath;
        public GameEvent OnPlayerNewLevel;
    #endregion

    public GameEvent _onLand;
    private void Awake()
    {
        player = GetComponent<Player>();
        rb = player.GetComponent<Rigidbody2D>();
        feedbacksManager = GameObject.Find("FeedbacksManager").GetComponent<FeedbacksManager>();
    }
    private void Start()
    {
        PlayerEnteredNewLevel();
    }

    private void PlayerEnteredNewLevel()
    {
        OnPlayerNewLevel.Raise(this, transform.position);
    }
    public void JumpIsDone()
    {
        player.animator.SetBool("isJumping", false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.relativeVelocity.y > feedbacksManager.minVelocityToPlayFeedback)
        {
            Debug.Log(collision.relativeVelocity.y);
            _onLand.Raise(this, null);
        }
            
    }
}
