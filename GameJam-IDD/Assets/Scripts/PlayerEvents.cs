using UnityEngine;
public class PlayerEvents : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rb;
    private FeedbacksManager feedbacksManager;
    private BoxCollider2D _boxCollider2d;
    public LayerMask groundObjects;
    private Animator anim;
    private AudioSource audioSource;
    private Vector3 sendEnemyHere;
    private Enemy enemy;
    
    [Header("Player Events")]
    public GameEvent OnPlayerDeath;
    public GameEvent OnPlayerNewLevel;
    public GameEvent _onLand;
    
    [Header("Check Ground")]
    public float groundCheckDistance;
    public bool playerIsFalling = false;
    public AudioClip jumpAudio;
    public AudioClip[] stepAudios;

    private void Awake()
    {
        player = GetComponent<Player>();
        rb = player.GetComponent<Rigidbody2D>();
        _boxCollider2d = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        feedbacksManager = GameObject.Find("FeedbacksManager").GetComponent<FeedbacksManager>();
        audioSource = gameObject.GetComponent<AudioSource>();
        if(GameObject.Find("SendEnemyHere"))
            sendEnemyHere = GameObject.Find("SendEnemyHere").GetComponent<Transform>().position;
        enemy = GameObject.FindObjectOfType<Enemy>();
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
    private void Update()
    {
        ThrowRaycastToKnowIfFalling();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.y > feedbacksManager.minVelocityToPlayFeedback)
        {
            _onLand.Raise(this, null);
        }
    }
    // Here it goes the code to fall and jump

    public void ThrowRaycastToKnowIfFalling()
    {
        Color c = Color.red;
        RaycastHit2D hit = Physics2D.BoxCast(_boxCollider2d.bounds.center, _boxCollider2d.bounds.size, 0f, Vector2.down, groundCheckDistance, groundObjects);
        if (hit.collider != null)
        {
            c = Color.blue;

            playerIsFalling = false;
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
            //StepSound();
        }
        else
        {
            c = Color.red;
            playerIsFalling = rb.velocity.y < 0;
            anim.SetBool("isFalling", rb.velocity.y < 0);
            anim.SetBool("isJumping", rb.velocity.y > 0);
        }
    }
    public void SwitchToFalling()
    {
        anim.SetBool("isFalling", true);
        anim.SetBool("isJumping", false);
    }
    public void StepSound()
    {
        float rndAudio = Random.Range(0, stepAudios.Length);
        audioSource.clip = stepAudios[(int)rndAudio];
        audioSource.PlayOneShot(audioSource.clip);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("SwitchEnemyTrigger"))
        {
            if(enemy != null)
            {
                enemy.SetEnemyState(EnemyState.Moving);
                enemy.SetEnemyDirection(EnemyDirection.Left);
                enemy.transform.position = sendEnemyHere;
            }
        }
    }
    public void JumpSound()
    {
        audioSource.clip = jumpAudio;
        audioSource.PlayOneShot(audioSource.clip);
    }
}
