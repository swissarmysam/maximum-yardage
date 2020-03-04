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

        if(jumpActive)
        {
            playerRigidBody.AddForce(new Vector2(0, jumpForce));
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
