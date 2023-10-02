using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Movement script for player using a Rigidbody2D
/// Features: acceleration/deceleration, Ground Check and Head Check, Coyote time, Jump Buffering, End Jump early
/// </summary>

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject graphics;


    [Header("WALKING")]
    [SerializeField] private float maxSpeed = 5f;                    // Base movement speed
    [SerializeField] private float acceleration = 10f;              // Acceleration factor
    [SerializeField] private float deceleration = 10f;               // Deceleration factor
    [SerializeField] private Transform groundCheck;                 // Transform of an object at the character's feet
    [SerializeField] private Transform headCheck;                   // Transform of an object at the character's head
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f); // Size of the ground check
    [SerializeField] private LayerMask groundLayer;                 // Layer mask for the ground

    [Header("JUMPING")][SerializeField] public float jumpHeight = 30;  // Amount of force added when player jumps     
    [SerializeField] private float coyoteTimeThreshold = 0.1f;      // How long player can jump for after leaving platform
    [SerializeField] private float jumpApexThreshold = 10f;         // Apex threshold
    [SerializeField] private float jumpBuffer = 0.1f;               // Amount of time player can input jump before landing and it still registers
    [SerializeField] private float jumpEndEarlyGravityModifier = 3; // How much downwards force is added when player releases jump early or hits a celing

    [Header("GRAVITY")]
    [SerializeField] private float minFallSpeed = 1f;
    [SerializeField] private float maxFallSpeed = 3f;
    [SerializeField] private float fallClamp = -40f;

    private Rigidbody2D rb;
    private float targetVelocityX = 0f;
    private float currentVelocityX = 0f;
    private float timeLeftGrounded;
    private float fallSpeed;


    private bool coyoteUsable;
    private bool endedJumpEarly = true;
    private float lastJumpPressed;
    private float currentVelocityY;
    private float apexPoint;
    private bool CanUseCoyote => coyoteUsable && !IsGrounded && timeLeftGrounded + coyoteTimeThreshold > Time.time;
    private bool HasBufferedJump => IsGrounded && lastJumpPressed + jumpBuffer > Time.time;
    public bool JumpingThisFrame { get; private set; }
    public float XAxis { get; private set; }
    public bool JumpDown { get; private set; }
    public bool JumpUp { get; private set; }
    public bool IsGrounded { get; private set; }
    public bool HitHead { get; private set; }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (rb == null) return;
        
        //IF GAME IS PAUSED SKIP THIS FRAME
        //if (PauseMenu.getGameIsPaused()) return;

        // Get input for movement
        GetInputs();
        CollisionCheck();
        // Calculate horizontal movement
        CalculateWalk();

        // Calculate Vertical movement
        CalculateGravity();
        CalculateJumpApex();
        CalculateJump();

        // Update the Rigidbody2D velocity
        Move();
        
        // Flip character sprite if moving left
        if (XAxis < 0)
        {
            graphics.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        // Flip character sprite if moving right
        else if (XAxis > 0)
        {
            graphics.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    // Gets player inputs needed to calculate movement
    private void GetInputs()
    {
        // Get input for movement
        XAxis = Input.GetAxis("Horizontal");
        JumpDown = Input.GetButtonDown("Jump");
        JumpUp = Input.GetButtonUp("Jump");
    }

    // Calculates the horizontal movement of the player
    private void CalculateWalk()
    {

        if (XAxis > 0)
        {
            targetVelocityX = XAxis * maxSpeed;
            currentVelocityX = Mathf.MoveTowards(currentVelocityX, targetVelocityX, acceleration * Time.deltaTime);
        }

        else if (XAxis < 0)
        {
            targetVelocityX = XAxis * maxSpeed;
            currentVelocityX = Mathf.MoveTowards(currentVelocityX, targetVelocityX, acceleration * Time.deltaTime);
        }

        else
        {
            targetVelocityX = 0f;
            currentVelocityX = Mathf.MoveTowards(currentVelocityX, targetVelocityX, deceleration * Time.deltaTime);
        }

    }

    // Checks for collisions with the ground check and celing check game objects
    private void CollisionCheck()
    {
        bool _groundedLastFrame = IsGrounded;
        IsGrounded = Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0f, groundLayer);
        if(_groundedLastFrame && !IsGrounded) 
        { 
        timeLeftGrounded = Time.time;
            if (!JumpingThisFrame)
            {
                coyoteUsable = true;
                
            }
            else
            {
                coyoteUsable = false;
            }
        }
        

        HitHead = Physics2D.OverlapBox(headCheck.position, groundCheckSize, 0f, groundLayer);
    }

    // Updates players velocity
    private void Move()
    {
        rb.velocity = new Vector2(currentVelocityX, currentVelocityY);
    }
    private void CalculateJumpApex()
    {
        if (!IsGrounded)
        {
            // Gets stronger the closer to the top of the jump
            apexPoint = Mathf.InverseLerp(jumpApexThreshold, 0, Mathf.Abs(rb.velocity.y));
            fallSpeed = Mathf.Lerp(minFallSpeed, maxFallSpeed, apexPoint);
        }
        else
        {
            apexPoint = 0;
        }
    }

    // Calculate the force added when player jumps or releases jump
    private void CalculateJump()
    {
        if (JumpDown)
        {
            lastJumpPressed = Time.time;
        }

        // Jump if: grounded or within coyote threshold || sufficient jump buffer
        if (JumpDown && CanUseCoyote || HasBufferedJump)
        {
            currentVelocityY = jumpHeight;
            endedJumpEarly = false;
            coyoteUsable = false;
            timeLeftGrounded = float.MinValue;
            JumpingThisFrame = true;
        }
        else
        {
            JumpingThisFrame = false;
        }

        // End the jump early if button released
        if (!IsGrounded && JumpUp && !endedJumpEarly && rb.velocity.y > 0)
        {
            // _currentVerticalSpeed = 0;
            endedJumpEarly = true;
        }

        if (HitHead)
        {
            if (currentVelocityY > 0) currentVelocityY = 0;
        }
    }

    // Calculates how Y velocity will change from gravity
    private void CalculateGravity()
    {
        if (IsGrounded)
        {
            // Move out of the ground
            if (currentVelocityY < 0) currentVelocityY = 0;
        }
        else
        {
            // Add downward force while ascending if we ended the jump early
            var _fallSpeed = endedJumpEarly && currentVelocityY > 0 ? fallSpeed * jumpEndEarlyGravityModifier : fallSpeed;

            // Fall
            currentVelocityY -= _fallSpeed * Time.deltaTime;

            // Clamp
            if (currentVelocityY < fallClamp) currentVelocityY = fallClamp;
        } 
    }


    // Draws gizmos for collision checks
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, new Vector3(groundCheckSize.x, groundCheckSize.y, 0.1f));
        Gizmos.DrawWireCube(headCheck.position, new Vector3(groundCheckSize.x, groundCheckSize.y, 0.1f));
    }

    public float getCurrentVelocityX ()
    {
        return currentVelocityX;
    }

}