using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public Transform cellingCheck;
    public Transform groundCheck;
    public LayerMask groundObjects;
    public float checkRadius;
    public float jumpDamping = 0.5f;
    public int maxJumpCount;

    private Rigidbody2D rb;
    private bool facingRight = true;
    private float moveDirection;
    private bool isJumping = false;
    private bool isGrounded = false;
    private int jumpCount;

    [Header("Jump")]
    public float groundCheckDistance;
    private float startGroundCheckDistance;
    private BoxCollider2D boxCollider2d;

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
        startGroundCheckDistance = groundCheckDistance;
    }

    // Update is called once per frame
    void Update()
    {
        //Get Inputs
        Inputs();

        //Animation
        Animate();

    }
    private void FixedUpdate()
    {
        //Check if ground
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundObjects);

        CheckGroundForJump(); // this function now takes into account the whole player collider when checking ground

        if (isGrounded)
        {
            jumpCount = maxJumpCount;
            ResetJump();
        }
        else
        {
            groundCheckDistance = 0f;
        }

        //Move
        SettingVelocity();
    }

    private void SettingVelocity()
    {
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
    }

    private void Inputs()
    {
        moveDirection = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount--;
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            jumpCount--;
        }

        isJumping = false; // ?
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
    private void ResetJump()
    {
        groundCheckDistance = startGroundCheckDistance;
    }
}
