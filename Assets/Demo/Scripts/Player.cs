using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Variables
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

    public Vector3 outsideImpulse = Vector3.zero;

    [SerializeField]
    private float baseBoostLevel = 100f;

    [SerializeField]
    private float outsideImpulseTreshold = 1f;

    [SerializeField]
    private float impulseDecay = 1f;


    [SerializeField]
    private float slopeForce;
    [SerializeField]
    private float slopeForceRayLength;

    // Functions
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        currentSpeed = moveSpeed;
    }
    private void Update()
    {
        // W A S D / Right Left Up Down Arrow Input
        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");
        // Left Shift Input

        // Space Bar Input
        bool inputJump = Input.GetButtonDown("Jump");
        // Put Horizontal & Vertical input into vector
        Vector3 inputDir = new Vector3(inputH, 0f, inputV);
        // Rotate direction to Player's Direction
        inputDir = transform.TransformDirection(inputDir);
        // If input exceeds length of 1
        if (inputDir.magnitude > 1f)
        {
            // Normalize it to 1f!
            inputDir.Normalize();
        }

        Move(inputDir.x, inputDir.z, currentSpeed);

        // If is Grounded
        if (controller.isGrounded)
        {
            // .. And jump?
            if (inputJump)
            {
                Jump(jumpHeight);
            }

            // Cancel the y velocity
            motion.y = 0f;

            // Is jumping bool set to true
            if (isJumping)
            {
                // Set jump height
                motion.y = currentJumpHeight;
                // Reset back to false
                isJumping = false;
            }
        }


        motion.y += gravity * Time.deltaTime;
        controller.Move((motion + outsideImpulse) * Time.deltaTime);

        if ((inputV != 0 || inputH != 0 && OnSlope()))
            controller.Move(Vector3.down * controller.height / 2 * slopeForce * Time.deltaTime);

        if ((controller.velocity.magnitude < outsideImpulseTreshold) || (controller.isGrounded))
        //if ((controller.velocity.magnitude < outsideImpulseTreshold))
        {
            outsideImpulse = Vector3.zero;
        }
        else
        {
            outsideImpulse = Vector3.Lerp(outsideImpulse, Vector3.zero, impulseDecay * Time.deltaTime);
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
        currentSpeed = startDash;

        yield return new WaitForSeconds(delay);

        currentSpeed = endDash;
    }
    public void Walk(float inputH, float inputV)
    {
        Move(inputH, inputV, moveSpeed);
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
        outsideImpulse = transform.TransformDirection(Vector3.forward) * baseBoostLevel * transform.localScale.x;
    }
    private bool OnSlope()
    {
        if (isJumping)
            return false;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, controller.height / 2 * slopeForceRayLength))
            if (hit.normal != Vector3.up)
                return true;
        return false;
    }
}

