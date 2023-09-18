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

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        jumpCount = maxJumpCount;
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
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundObjects);

        if (isGrounded)
        {
            jumpCount = maxJumpCount;
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

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0 && jumpCount > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpDamping);
            jumpCount--;
        }

        isJumping = false;
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
}
