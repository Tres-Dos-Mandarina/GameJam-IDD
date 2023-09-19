using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public float acceleration;
    public float moveSpeed;
    public float moveSpeedRunning;
    public float airSpeed;
    public float airSpeedRunning;
    public float jumpForce;
    public float wallJumpLerp = 10;

    public LayerMask groundObjects;
    public float checkRadius;
    public float jumpDamping = 0.5f;
    public int maxJumpCount;

    private Rigidbody2D rb;
    private bool facingRight = true;
    private float startMoveSpeed;
    private float startAirMoveSpeed;
    private float moveDirection;
    private bool isGrounded = false;
    private int jumpCount;
    private float currentSpeed = 0.0f;

    [Header("Acceleration and Deceleration")]
    public float accelerationTime = 0.1f; // Tiempo de aceleración en segundos (6 frames a 60 FPS)
    public float decelerationTime = 3.0f; // Tiempo de desaceleración en segundos
    private float accelerationTimer;
    private float decelerationTimer;
    private float maxSpeed;

    [Header("Jump")]
    public float groundCheckDistance;
    private BoxCollider2D boxCollider2d;
    private SpriteRenderer sr;

    bool canMove = true;
    bool wallGrab = false;
    bool wallJumped = false;
    bool onWall = false;

    // Start is called before the first frame update
    void Awake()
    {
        #region GetComponents
        rb = GetComponent<Rigidbody2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        #endregion
        #region GameObject.Find()
        #endregion
    }
    private void Start()
    {
        jumpCount = maxJumpCount;
        startMoveSpeed = moveSpeed;
        startAirMoveSpeed = airSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //Get Inputs
        Movement();

        //Animation
        Animate();

    }
    private void FixedUpdate()
    {
        //Check if ground
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundObjects);

        CheckGroundForJump(); // this function now takes into account the whole player collider when checking ground
        
        WallCheck();

        Debug.Log(onWall);

        if (isGrounded)
        {
            jumpCount = maxJumpCount;
        }
    }
    private void WallCheck()
    {
        Vector2 rayDirection = facingRight ? Vector2.right : Vector2.left;

        Vector2 rayOrigin = new Vector2(transform.position.x + 0.3f, transform.position.y);

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, 2000f);
        Debug.Log(hit.point);
        Debug.DrawRay(rayOrigin, rayDirection * 0.1f, Color.red);
        /*if(hit.transform.tag != "Player")
        {
            onWall = true;
        }
        else
            onWall = false;*/
    }
    private void Walk(Vector2 dir)
    {
        if (!canMove)
            return;

        if (wallGrab)
            return;

        if (!wallJumped)
        {
            rb.velocity = new Vector2(dir.x * moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(dir.x * moveSpeed, rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        }
    }


    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);

        if (!isGrounded)
            dir /= airSpeed;

        Walk(dir);

        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity += Vector2.up * jumpForce;            

            jumpCount--;
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            jumpCount--;
        }

        if(Input.GetButtonDown("Sprint"))
        {
            moveSpeed = moveSpeedRunning;
            airSpeed = airSpeedRunning; 
        }
        if (Input.GetButtonUp("Sprint"))
        {
            moveSpeed = startMoveSpeed;
            airSpeed = startAirMoveSpeed;
        }

    }

    private void Animate()
    {
        if (moveDirection > 0 && !facingRight)
            Flip();
        else if (moveDirection < 0 && facingRight)
            Flip();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
    private void CheckGroundForJump()
    {
        Color c = Color.red;
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size,0f,Vector2.down,groundCheckDistance, groundObjects);
        if (hit.collider != null)
        {
            c = Color.blue;
            isGrounded = true;
        }
        else
        {
            c = Color.red;
            isGrounded = false;
        }
        Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + groundCheckDistance), c);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + groundCheckDistance), c);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, boxCollider2d.bounds.extents.y+groundCheckDistance), Vector2.down * (boxCollider2d.bounds.extents.y + groundCheckDistance), c);
    }
}
