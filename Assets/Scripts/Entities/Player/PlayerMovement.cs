using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("WALKING")]
    [SerializeField] public float maxSpeed = 5f;                    // Base movement speed
    [SerializeField] private float acceleration = 10f;              // Acceleration factor
    [SerializeField] public float deceleration = 10f;               // Deceleration factor
    [SerializeField] private Transform groundCheck;                 // Transform of an object at the character's feet
    [SerializeField] private Transform headCheck;                   // Transform of an object at the character's head
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.5f, 0.1f); // Size of the ground check
    [SerializeField] private LayerMask groundLayer;                 // Layer mask for the ground

    [Header("JUMPING")][SerializeField] public float jumpHeight = 30;  
    [SerializeField] private float jumpApexThreshold = 10f;
    [SerializeField] private float coyoteTimeThreshold = 0.1f;
    [SerializeField] private float jumpBuffer = 0.1f;
    [SerializeField] private float jumpEndEarlyGravityModifier = 3;

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
    private float apexPoint; // Becomes 1 at the apex of a jump
    private float lastJumpPressed;
    private float currentVelocityY;
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
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        // Flip character sprite if moving right
        else if (XAxis > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void GetInputs()
    {
        // Get input for movement
        XAxis = Input.GetAxis("Horizontal");
        JumpDown = Input.GetButtonDown("Jump");
        JumpUp = Input.GetButtonUp("Jump");
    }

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, new Vector3(groundCheckSize.x, groundCheckSize.y, 0.1f));
        Gizmos.DrawWireCube(headCheck.position, new Vector3(groundCheckSize.x, groundCheckSize.y, 0.1f));
    }

}