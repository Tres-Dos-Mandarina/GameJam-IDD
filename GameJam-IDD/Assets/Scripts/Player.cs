using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    public float acceleration;
    public float moveSpeed;
    public float moveSpeedRunning;
    public float airSpeed;
    public float airSpeedRunning;
    public float jumpForce;
    public float wallJumpLerp = 10;
    public float slideSpeed = 5;

    public LayerMask groundObjects;
    public LayerMask walls;
    private bool _onRightWall;
    private bool _onLeftWall;
    public float checkRadius;
    public float jumpDamping = 0.5f;

    private Rigidbody2D _rb;
    private bool _facingRight = true;
    private float _startMoveSpeed;
    private float _startAirMoveSpeed;
    public bool _isGrounded = false;

    [SerializeField]
    private float _coyoteTime; //Tiempo en el que el jugador puede saltar despues caer de una plataforma
    private float _coyoteTimeCounter;

    [SerializeField]
    private float _jumpBufferTime; //Tiempo en el que salta al tocar el suelo si ha saltado el player en el aire
    private float _jumpBufferCounter;

    [Header("Acceleration and Deceleration")]
    //public float accelerationTime = 0.1f; // Tiempo de aceleración en segundos (6 frames a 60 FPS)
    //public float decelerationTime = 3.0f; // Tiempo de desaceleración en segundos
    //private float _accelerationTimer;
    //private float _decelerationTimer;
    //private float _maxSpeed;
    private float lastSpeed;


    [Header("Jump")]
    public float groundCheckDistance;
    private BoxCollider2D _boxCollider2d;
    private SpriteRenderer _sr;

    public bool _canMove = true;
    private bool _wallGrab = false;
    private bool _wallSlide = false;
    private bool _wallJumped = false;
    private bool _onWall = false;
    private bool _groundTouch = false;

    GameEvent _onGoalReached;
    public GameEvent _onJump;

    // Start is called before the first frame update
    void Awake()
    {
        #region GetComponents
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider2d = GetComponent<BoxCollider2D>();
        #endregion
        #region GameObject.Find()
        #endregion
    }
    private void Start()
    {
        _startMoveSpeed = moveSpeed;
        _startAirMoveSpeed = airSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        Movement();
    }
    private void FixedUpdate()
    {
        //Check if ground
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundObjects);

        CheckGroundForJump(); // this function now takes into account the whole player collider when checking ground
        
        //WallCheck();
        
    }
    private void WallJump()
    {
        StopCoroutine(DisableMovement(0));
        StartCoroutine(DisableMovement(.1f));
    
        Vector2 wallDir = _onRightWall ? Vector2.left : Vector2.right;
        animator.SetBool("isJumping", true);
        animator.SetBool("isSliding", false);
        BasicJump((Vector2.up / 2f + wallDir / 2f), true);
    
        _wallJumped = true;
    }
    public IEnumerator DisableMovement(float time)
    {
        _canMove = false;
        yield return new WaitForSeconds(time);
        _canMove = true;
    }
    public void DisableMovement()
    {
        _canMove = false;
    }
    private void Walk(Vector2 dir)
    {
        if (!_canMove)
            return;

        if (_wallGrab)
            return;

        if (Mathf.Abs(dir.x) < lastSpeed) dir.x = 0;


        animator.SetFloat("Speed", Mathf.Abs(dir.x));

        if (!_wallJumped)
        {
            _rb.velocity = new Vector2(dir.x * moveSpeed, _rb.velocity.y);
        }
        else
        {
            _rb.velocity = Vector2.Lerp(_rb.velocity, (new Vector2(dir.x * moveSpeed, _rb.velocity.y)), wallJumpLerp * Time.deltaTime);
        }
    }
    private void Movement()
    {
        Vector2 dir = new Vector2(0, 0);

        dir.x = Input.GetAxis("Horizontal");
        dir.y = Input.GetAxis("Vertical");

        if((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) && _isGrounded)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        
        Animate(dir.x);

        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");

        if (!_isGrounded)
            dir /= airSpeed;

        Walk(dir);
        lastSpeed = Mathf.Abs(dir.x);
        
        if (_onWall && ((dir.x > 0.5f && _onRightWall) || (dir.x < -0.5f && _onLeftWall)) && _canMove)
        {
            _wallGrab = true;
            animator.SetBool("isSliding", true);
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
            _wallSlide = false;
        }
        else if ((dir.x < 0.4 || dir.x > -0.4) || !_onWall)
        {
            _wallGrab = false;
            animator.SetBool("isSliding", false);
            _wallSlide = false;
        }

        if (_wallGrab)
        {
            //rb.gravityScale = 0;
            //if (x > .2f || x < -.2f)
            //    rb.velocity = new Vector2(rb.velocity.x, 0);

            //float speedModifier = y > 0 ? .5f : 1;

            //rb.velocity = new Vector2(rb.velocity.x, y * (moveSpeed * speedModifier));
            WallSlide(slideSpeed/4.0f);
        }
        else
        {
            _rb.gravityScale = 3;
        }        

        if (_onWall && _isGrounded)
        {
            _onWall = false;
            _isGrounded = true;
            if (dir.x > 0.8f || dir.x < -0.8f)
            {
                _onWall = true;
                _isGrounded = false;
            }
        }
        else
        {
            if (_onWall && !_isGrounded)
            {
                if (dir.x != 0 && !_wallGrab)
                {
                    _wallSlide = true;
                    WallSlide(slideSpeed);
                }
            }
        }
        //if(_isGrounded)
        //{
        //    animator.SetBool("isJumping",false);
        //}

        if (!_onWall || _isGrounded)
        {
            _wallSlide = false;
        }

        //Coyote Time
        if (_isGrounded)
        {
            _coyoteTimeCounter = _coyoteTime;
        }
        else 
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }

        //Jump Buffering
        if (Input.GetButtonDown("Jump"))
        {
            _jumpBufferCounter = _jumpBufferTime;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }

        if (_jumpBufferCounter > 0)
        {

            if (_onWall && _coyoteTimeCounter <= 0) 
            {
                WallJump(); // this means that the player is touching a wall and must wall jump
                _jumpBufferCounter = 0;
            }
            else if(_coyoteTimeCounter > 0)
            {
                BasicJump(Vector2.up, false);
                _jumpBufferCounter = 0;
            }
        }

        if (Input.GetButtonUp("Jump") && _rb.velocity.y > 0)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0f);

            _coyoteTimeCounter = 0f;
        }

        if(Input.GetButtonDown("Sprint"))
        {
            moveSpeed = moveSpeedRunning;
            airSpeed = airSpeedRunning; 
        }
        if (Input.GetButtonUp("Sprint"))
        {
            moveSpeed = _startMoveSpeed;
            airSpeed = _startAirMoveSpeed;
        }

        _onWall = Physics2D.OverlapCircle(new Vector2(transform.position.x+ .400f, transform.position.y), .15f, walls)
            || Physics2D.OverlapCircle(new Vector2(transform.position.x - .300f, transform.position.y), .15f, walls);
        _onRightWall = Physics2D.OverlapCircle(new Vector2(transform.position.x + .400f, transform.position.y), .15f, walls);
        _onLeftWall = Physics2D.OverlapCircle(new Vector2(transform.position.x - .300f, transform.position.y), .15f, walls);

        

    }
    private void WallSlide(float speed)
    {
        if (!_canMove)
            return;

        bool pushingWall = false;
        if ((_rb.velocity.x > 0 && _onRightWall) || (_rb.velocity.x < 0 && _onLeftWall))
        {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : _rb.velocity.x;

        _rb.velocity = new Vector2(push, -speed);
    }
    private void Animate(float _moveDirection)
    {
        if (_moveDirection > 0 && !_facingRight)
            Flip();
        else if (_moveDirection < 0 && _facingRight)
            Flip();
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(new Vector2(transform.position.x + .400f, transform.position.y), .15f);
        Gizmos.DrawSphere(new Vector2(transform.position.x - .300f, transform.position.y), .15f);
    }
    private void BasicJump(Vector2 dir, bool wall)
    {
        animator.SetBool("isJumping", true);
        if(wall)
        {
            float newForce = jumpForce * 2f;
            _rb.velocity = new Vector2(_rb.velocity.x, 0f);
            _rb.velocity += dir * newForce;
        }
        else
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0f);
            _rb.velocity += dir * jumpForce;
            _onJump.Raise(this, "jumped");
        }
    }
    private void Flip()
    {
        _facingRight = !_facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
    public void CheckGroundForJump()
    {
        Color c = Color.red;
        RaycastHit2D hit = Physics2D.BoxCast(_boxCollider2d.bounds.center, _boxCollider2d.bounds.size,0f,Vector2.down,groundCheckDistance, groundObjects);
        if (hit.collider != null)
        {
            c = Color.blue;
            _wallJumped = false;
            _isGrounded = true;
        }
        else
        {
            c = Color.red;
            _isGrounded = false;
        }
    }
}
