using System.Collections.Generic;
using UnityEngine;

public class Movement2 : MonoBehaviour
{
    // Movement Settings
    [Header("Movement Settings")]
    [Tooltip("Normalized input vector for movement direction.")]
    public Vector3 NormalizedInputVector;
    public FootStepsCode footStepsCode;

    [Tooltip("Movement speed of the character.")]
    public float MoveSpeed = 5f;
    [Tooltip("The maximum angle that the player will use slope movement for (anything beyond this will be treated as steps)")]
    public float MaxSlopeAngle = 30;

    [Tooltip("The amount of drag the player experiences while in the air.")]
    public float AirDrag = 0.9f;
    [Tooltip("The maximum speed that the player will be able to reach in the air without assistance.")]
    public float AirMoveSpeed = 0.1f;

    // Jump Settings
    [Header("Jump Settings")]
    [Tooltip("Strength of the jump force.")]
    public float JumpStrength;

    [Tooltip("Can the player jump?")]
    public bool CanJump;
    [Tooltip("The time since last jump")]
    public float TimeSinceJumped;

    // height adjustment settings
    [Header("Height adjustment")]
    [Tooltip("Raycast distance to detect the height of the ground")]
    public float RcastDistance = 1.2f;
    [Tooltip("the number of previous height values to average with the new value")]
    public int numberOfValues = 10;
    // Ground Detection
    [Header("Ground Detection")]
    [Tooltip("Time required to be on the ground to run late ground code")]
    public float RequiredGroundTime = 2.0f;

    [Tooltip("The amount of time to force the player to not be grounded after they have jumped")]
    public float DontGroundAfterJump;

    [Tooltip("Is the character currently on the ground?")]
    public bool OnGround;


    // Collision Settings
    [Header("Collision Settings")]
    [Tooltip("Reference to the trigger collider that detects ground.")]
    public TriggerCounter triggerCounter;

    [Tooltip("Reference for the collider that gets disabled when on the ground")]
    public Collider ColliderMain;
    [Tooltip("The averaged velocity of any object(s) the player is standing on that have rigidbodys")]
    public Vector3 MovementUnderPlayer;
    [Header("Display properties")]
    [SerializeField]
    private Vector3 currentVelocity;

    // Private Fields (not shown in Inspector)
    private Vector3 PreservedVelocityXZ;
    private RaycastHit StepDownDetectRcast;
    private Vector3 InputVelDirection;
    private float HeightTarget;
    private Queue<float> previousYPositions = new Queue<float>();
    private bool JustPressedJump = false;
    public bool AttemptJump = false;
    private float ElapsedGroundTime = 0.0f;
    public Rigidbody rb;
    private RaycastHit RcastHit;

    private void Awake()
    {
        // Initialize Rigidbody reference
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {

    }
    void Update()
    {
        CalculateInputs();
        currentVelocity = rb.linearVelocity;
    }
    void FixedUpdate()
    {
        AddNewYPosition(rb.position.y);
        GroundCheck();
        Jump();
        GroundedHeightControl();
        MoveGround();
        MoveAir();
        ApplyCustomDrag();
        RespawnIfInVoid();
    }
    void CalculateInputs()
    {
        // Get input from WASD keys
        float horizontal = Input.GetAxisRaw("Horizontal"); // A, D, Left Arrow, Right Arrow
        float vertical = Input.GetAxisRaw("Vertical"); // W, S, Up Arrow, Down Arrow

        // Create a new direction vector based on input
        NormalizedInputVector = new Vector3(horizontal, 0f, vertical).normalized;
        // Rotate the vector to algin with the curently faced direction
        NormalizedInputVector = transform.TransformDirection(NormalizedInputVector);

        // Register jump inputs
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AttemptJump = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            AttemptJump = false;
        }
    }
    void MoveGround()
    {
        if (OnGround)
        {
            // Set movement under player to be the velocity of what the player is standing on
            MovementUnderPlayer = triggerCounter.averageLinearVelocity;
            // Create a movement vector and project it onto plane
            InputVelDirection = NormalizedInputVector * MoveSpeed;
            if (OnGround && !JustPressedJump && Vector3.Angle(RcastHit.normal, Vector3.up) < MaxSlopeAngle)
            {
                InputVelDirection = Vector3.ProjectOnPlane(InputVelDirection, RcastHit.normal);
            }
            // set player velocity
            rb.linearVelocity = InputVelDirection + MovementUnderPlayer;
            Debug.DrawRay(transform.position, InputVelDirection);
        }
        if (OnGround && NormalizedInputVector != Vector3.zero && footStepsCode != null)
        {
            footStepsCode.IsWalking = true;
        }
        else
        {
            footStepsCode.IsWalking = false;
        }
    }
    void MoveAir()
    {
        if (!OnGround)
        {
            Vector3 AirInputVel;
            AirInputVel = NormalizedInputVector * AirMoveSpeed;
            AirInputVel.y = 0f;
            if (Vector3.Magnitude(new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z) + AirInputVel) < MoveSpeed)
            {
                rb.linearVelocity = rb.linearVelocity + AirInputVel;
            }
        }
    }
    void Jump()
    {
        if (CanJump && AttemptJump)
        {
            JustPressedJump = true;
            TimeSinceJumped = 0f;
            Vector3 jumpDir = (RcastHit.normal != Vector3.zero) ? RcastHit.normal : Vector3.up;
            //rb.AddForce(jumpDir * JumpStrength, ForceMode.Impulse);
            rb.linearVelocity = rb.linearVelocity + (jumpDir * JumpStrength);
            PreservedVelocityXZ = rb.linearVelocity;
            PreservedVelocityXZ.y = 0;


            CanJump = false;
            OnGround = false;
        }
        AttemptJump = false;
    }
    void GroundCheck()
    {
        // Keep us un-grounded if we have just jumped
        if (JustPressedJump)
        {
            OnGround = false;
            // Incerment time since jump
            if (TimeSinceJumped <= DontGroundAfterJump)
            {
                TimeSinceJumped += Time.deltaTime;
            }
            else // The time since jump is high enough, go back to nromal ground detection
            {
                JustPressedJump = false;
            }
        }
        else // Run normal ground detction
        {
            // Check if we are hitting the ground after not being on the ground
            if (OnGround == false && triggerCounter.ObjectCount > 0f)
            {
                JustHitGround();
            }
            // Check if we are currently on the ground
            if (triggerCounter.ObjectCount > 0)
            {
                OnGround = true;
            }
            else
            {
                OnGround = false;
            }
        }
        // Do on ground things (that count as detection)
        if (OnGround)
        {
            // Run timer to check if we have been on the ground for RequiredGroundTime
            ElapsedGroundTime += Time.deltaTime;
            if (ElapsedGroundTime >= RequiredGroundTime)
            {
                // Timer is up, start running code that needs the player to have been on the ground
                BeenOnGround();
            }
        }
        else
        {
            ElapsedGroundTime = 0f;  // Reset the timer if we leave the ground
        }
    }
    void GroundedHeightControl()
    {
        if (OnGround && !JustPressedJump)
        {
            rb.useGravity = false;

            if (Physics.Raycast(transform.position, Vector3.down, out RcastHit, RcastDistance))
            {
                float timecheck = 0;
                ColliderMain.enabled = false;
                if (timecheck - Time.time < 0.2f)
                {
                    ResetYPositions(transform.position.y);
                }

                timecheck = Time.time;
                if (Mathf.Abs(rb.linearVelocity.y) < 200)
                {
                    SelectTargetHeight();
                    rb.MovePosition(new Vector3(transform.position.x, HeightTarget, transform.position.z));
                }
            }
            else
            {
            }

        }
        if (!OnGround)
        {
            ColliderMain.enabled = false;
            rb.useGravity = true;
        }
    }
    void JustHitGround()
    {
        footStepsCode.OnLand(rb.linearVelocity.y);
        CanJump = true;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        ResetYPositions(transform.position.y);
    }
    void BeenOnGround()
    {

    }
    void SelectTargetHeight()
    {
        HeightTarget = AddNewYPosition((RcastHit.point.y + 1f));
    }
    void RespawnIfInVoid()
    {
        if (transform.position.y < -200)
        {
            transform.position = Vector3.zero;
        }
    }
    void ApplyCustomDrag()
    {
        Vector3 dragForce = -AirDrag * rb.linearVelocity;
        dragForce.y = 0;
        rb.linearVelocity += dragForce * Time.fixedDeltaTime;
    }
    public float AddNewYPosition(float newY)
    {
        // Add the new Y position to the list
        previousYPositions.Enqueue(newY);

        // If we have more than the desired number of values, remove the oldest
        if (previousYPositions.Count > numberOfValues)
        {
            previousYPositions.Dequeue();
        }

        // Calculate the average
        float sum = 0f;
        foreach (float y in previousYPositions)
        {
            sum += y;
        }
        return sum / previousYPositions.Count;
    }
    public void ResetYPositions(float currentY)
    {
        // Clear the queue
        previousYPositions.Clear();

        // Fill the queue with the current Y value
        for (int i = 0; i < numberOfValues; i++)
        {
            previousYPositions.Enqueue(currentY);
        }
    }
}
