using UnityEngine;

public class Movement : MonoBehaviour
{
    private float originalCameraHeight;
    public Camera playerCamera;
    private bool isCrouching;
    private Animator animator;
    float velocityZ = 0.0f;
    float velocityX = 0.0f;
    public float acceleration = 2.0f;
    public float deceleration = 2.0f;
    public float maximumWalkVelocity = 0.5f;
    public float maximumRunVelocity = 2.0f;
    public float groundCheckDistance = 1.0f;
    public float jumpForce = 5.0f;
    public float crouchHeight = 0.5f;
    public float standingHeight = 2.0f;
    private CharacterController characterController;
    private Vector3 moveDirection;
    
    void Start()
    {
        
        // Store the original camera height at the start of the script
        originalCameraHeight = playerCamera.transform.localPosition.y;
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

    }

    void Update()
    {

        //input will be true if player is pressing on the passed in key parameter
        bool forwardPressed = Input.GetKey("w");
        bool leftPressed  = Input.GetKey("a");
        bool rightPressed = Input.GetKey("d");
        bool backPressed = Input.GetKey("s");
        bool runPressed = Input.GetKey("left shift");
        
        bool jumpPressed = Input.GetKeyDown("space");

        bool isIdle = velocityZ == 0.0f && velocityX == 0.0f;

        float currentMaxVelocity = runPressed ? maximumRunVelocity : maximumWalkVelocity;
        float pistolLayerWeight = animator.GetLayerWeight(1);
        //accleration of movement
        if (forwardPressed)
        {
            velocityZ = Mathf.Min(velocityZ + Time.deltaTime * acceleration, currentMaxVelocity);
        }
        if (backPressed)
        {
            velocityZ = Mathf.Max(velocityZ - Time.deltaTime * acceleration, -currentMaxVelocity);
        }
        if (leftPressed && !runPressed)
        {
            velocityX = Mathf.Max(velocityX - Time.deltaTime * acceleration, -maximumWalkVelocity);
        }
        if (rightPressed && !runPressed)
        {
            velocityX = Mathf.Min(velocityX + Time.deltaTime * acceleration, maximumWalkVelocity);
        }

        //reset velocityZ
        if (!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ = 0.0f;
        }

        //deceleration
        if (!forwardPressed && !backPressed)
        {
            velocityZ = Mathf.MoveTowards(velocityZ, 0.0f, Time.deltaTime * deceleration);
        }
        if (!leftPressed && !rightPressed)
        {
            velocityX = Mathf.MoveTowards(velocityX, 0.0f, Time.deltaTime * deceleration);
        }

        //reset VelocityX
        if (!leftPressed && !rightPressed && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0.0f;
        }
        if (jumpPressed && characterController.isGrounded)
        {
            moveDirection.y = jumpForce;
            animator.SetTrigger("Jump");
        }
        if (Input.GetKeyDown("c"))
        {

            if (pistolLayerWeight == 0)
            {
                isCrouching = !isCrouching;
                animator.SetBool("Crouch", isCrouching);
                playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, isCrouching ? crouchHeight : originalCameraHeight, playerCamera.transform.localPosition.z);
                characterController.height = isCrouching ? crouchHeight : standingHeight;
            }
        }
        if (pistolLayerWeight > 0 && isCrouching)
        {
            StandUp();
        }
        animator.SetFloat("Velocity X", velocityX);
        animator.SetFloat("Velocity Z", velocityZ);
        animator.SetBool("IsIdle", isIdle);
        AdjustToTerrain();
    }
    private void StandUp()
    {
        isCrouching = false;
        animator.SetBool("Crouch", isCrouching);
        playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, originalCameraHeight, playerCamera.transform.localPosition.z);
        characterController.height = standingHeight;
    }
    void AdjustToTerrain()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position + Vector3.up * 0.1f;
        float yOffset = 0.1f; // Add a small offset to the targetHeight

        if (Physics.Raycast(raycastOrigin, Vector3.down, out hit, groundCheckDistance))
        {
            if (hit.collider.CompareTag("Terrain")) // Adjust the "Terrain" tag to match your terrain's tag
            {
                float targetHeight = hit.point.y + yOffset;
                transform.position = new Vector3(transform.position.x, targetHeight, transform.position.z);
            }
        }
    }



}
