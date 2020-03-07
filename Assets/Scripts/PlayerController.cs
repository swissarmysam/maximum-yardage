using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // force to apply to jump
    public float jumpForce = 125.0f;
    public float forwardSpeed = 3.0f;
    private Rigidbody2D playerRigidBody;

    // variables for groundCheck for animation transition
    // this will store a reference to the groundCheck child object
    public Transform groundCheckTransform;
    private bool isGrounded;
    // this will store the Layer which is defined as ground (floor)
    public LayerMask groundCheckLayerMask;
    // this will be a reference to the animator component
    private Animator playerAnimator;

    // bool var for stunned status
    // private bool isStunned = false;

    // set bool flag for limiting jump action
    public bool jumpLimit = false;

    // scrolling background
    public ParallaxScroll parallax;

    // Start is called before the first frame update
    void Start()
    {

        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();

    }

    void FixedUpdate()
    {

        // see if jump action was made
        bool jumpActive = Input.GetButton("Fire1");

        // jump is only registered if the player isGrounded - prevents jump from being held down
        if (jumpActive && isGrounded)
        {
            // limit the amount of velocity added to the jump
            if(playerRigidBody.velocity.y <= 10)
            {
                playerRigidBody.AddForce(new Vector2(0, jumpForce));
            }
        }

        Vector2 newVelocity = playerRigidBody.velocity;
        newVelocity.x = forwardSpeed;
        playerRigidBody.velocity = newVelocity;

        // keep checking if player is grounded
        UpdateGroundedStatus();

        parallax.offset = transform.position.x;
    }

    void UpdateGroundedStatus()
    {
        // if the groundCheck object overlaps the ground layer then the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);
        // this updates the animator isGrounded parameter
        playerAnimator.SetBool("isGrounded", isGrounded);
    }

}
