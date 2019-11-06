using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Variables
    public const string playerTag = "Player";

    public float moveSpeed = 20f;
    public float dashSpeed = 20f;
    public float dashTime = 2f;
    public float gravity = -10f;
    public float jumpHeight = 15f;
    public float groundRayDistance = 1.1f;
    private CharacterController controller; // Reference to character controller
    private Vector3 motion; // Is the movement offset per frame
    private bool isJumping;
    private float currentJumpHeight;
    private float currentSpeed;
    private bool canAirJump;
    private bool needToEndBoost = false; // Bool to keep track if we need to end the speed boost, or something else interrupted it

    public Vector3 outsideImpulse = Vector3.zero;

    [SerializeField]
    private float baseBoostLevel = 100f;

    [SerializeField]
    private float outsideImpulseTreshold = 1f;

    [SerializeField]
    private float impulseDecay = 2f;

    [SerializeField]
    private float airImpulseDecay = 1f;

    [SerializeField]
    private float coyoteTime = 0f;

    private float coyoteTimer = 0f;

    [SerializeField]
    private float jumpQueueTime = 0f;

    private float jumpQueueTimer = 0f;

    [SerializeField]
    private float strafeSpeedMultiplier = 1f;

    [SerializeField]
    private bool gottaGoFast = false;

    public bool IsGrounded
    {
        get { return controller.isGrounded; }
    }

    // Functions
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = moveSpeed;

        // The initial position of the player acts as the first checkpoint
        CheckPointManager.Instance.CheckpointReached(transform);

        if (Application.isEditor)
        {
            GameObject playerLoc = new GameObject("SpawnLocation");
            Transform playerLocTrans = playerLoc.GetComponent<Transform>();
            playerLocTrans.position = transform.position;
            playerLocTrans.rotation = transform.rotation;
            CheckPointManager.Instance.RegisterSpawnLocation(0, playerLocTrans);
        }
    }
    private void Update()
    {
        if (InputManager.Instance.GetButtonDown(InputManager.InputKeys.QuickRestart))
        {
            // If the player presses the restart button, respawn the player at the last checkpoint
            CheckPointManager.Instance.StartRespawnSequence();
            return;
        }

        // W A S D / Right Left Up Down Arrow Input
        float inputH = InputManager.Instance.GetAxis(InputManager.AxisInputs.MoveHorizontal);
        float inputV = InputManager.Instance.GetAxis(InputManager.AxisInputs.MoveVertical);
        // Left Shift Input

        // Space Bar Input
        bool inputJump = false;
        if (InputManager.Instance.GetButtonDown(InputManager.InputKeys.Jump))
        {
            inputJump = true;
            jumpQueueTimer = jumpQueueTime;
        }
        else
        {
            jumpQueueTimer = Mathf.Max(0f, jumpQueueTimer - Time.deltaTime);
        }
        inputJump = inputJump || (jumpQueueTimer > 0f);
        // Put Horizontal & Vertical input into vector
        Vector3 inputDir = new Vector3(inputH, 0f, (gottaGoFast ? 1f : inputV));
        // Rotate direction to Player's Direction
        inputDir = transform.TransformDirection(inputDir);
        // If input exceeds length of 1
        if (inputDir.magnitude > 1f)
        {
            // Normalize it to 1f!
            inputDir.Normalize();
        }

        Move(inputDir.x * strafeSpeedMultiplier, inputDir.z, currentSpeed);

        if (controller.isGrounded)
        {
            coyoteTimer = coyoteTime;
            canAirJump = false;
        }
        else
        {
            coyoteTimer = Mathf.Max(0f, coyoteTimer - Time.deltaTime);
        }

        // If is Grounded
        if ((coyoteTimer > 0f) || canAirJump)
        {
            // .. And jump?
            if (!isJumping && (inputJump))
            {
                jumpQueueTimer = 0f;
                Jump(jumpHeight);
                currentJumpHeight *= (canAirJump ? 2f : 1f);
            }

            // Cancel the y velocity if grounded
            if (controller.isGrounded)
            {
                motion.y = 0f;
            }

            // Is jumping bool set to true
            if (isJumping)
            {
                
                // Set jump height
                motion.y = currentJumpHeight;
                // Reset back to false
                isJumping = false;
                canAirJump = false;
            }
        }


        motion.y += gravity * Time.deltaTime;
        controller.Move((motion + outsideImpulse) * Time.deltaTime);

        for (int i = 0; i < 3; ++i)
        {
            if (((currentSpeed * outsideImpulseTreshold) > Mathf.Abs(outsideImpulse[i])) && controller.isGrounded)
            {
                outsideImpulse[i] = 0f;
            }
            else
            {
                outsideImpulse[i] = Mathf.Lerp(outsideImpulse[i], 0f, (controller.isGrounded ? impulseDecay : airImpulseDecay) * Time.deltaTime);
            }

            if (((i == 1) || (Mathf.Abs(outsideImpulse[i]) < currentSpeed * 2f)) && (((controller.velocity[i] < 0f) && (outsideImpulse[i] > 0f)) || ((controller.velocity[i] > 0f) && (outsideImpulse[i] < 0f))))
            {
                motion[i] += outsideImpulse[i];
                outsideImpulse[i] = 0f;
            }
        }

    }
    private void Move(float inputH, float inputV, float speed)
    {
        Vector3 direction = new Vector3(inputH, 0f, inputV);
        motion.x = direction.x * speed;
        motion.z = direction.z * speed;
    }
    IEnumerator SpeedBoost(float startDash, float endDash, float delay)
    {
        needToEndBoost = true;
        currentSpeed = startDash;

        yield return new WaitForSeconds(delay);

        if (needToEndBoost)
        {
            needToEndBoost = false;
            currentSpeed = endDash;
        }
    }
    public void Jump(float height)
    {
        isJumping = true; // We are jumping!
        currentJumpHeight = height;
    }
    public void Dash()
    {
        StartCoroutine(SpeedBoost(dashSpeed, moveSpeed, dashTime));
    }
    public void Boost(Transform transform)
    {
        Boost(transform.TransformDirection(Vector3.forward) * transform.localScale.x);
    }
    public void Boost(Vector3 boost)
    {
        motion.y = 0f;
        outsideImpulse = boost * baseBoostLevel;
    }
    public void AirJump()
    {
        canAirJump = true;
    }
    public void Warp(Vector3 loc)
    {
        controller.enabled = false;
        transform.position = loc;
        controller.enabled = true;
    }
    public void ResetPlayer()
    {
        // Reset Player variables to start state
        motion = Vector3.zero;
        isJumping = false;
        currentJumpHeight = jumpHeight;
        currentSpeed = moveSpeed;
        canAirJump = false;
        outsideImpulse = Vector3.zero;
        needToEndBoost = false;
    }
}
