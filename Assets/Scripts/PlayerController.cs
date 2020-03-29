using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public bool isStunned = false;

    // scrolling background
    public ParallaxScroll parallax;

    // variables to track and display score
    private float yardsRun = 0;
    private float multiplier = 2.0f;
    public Text yardsRunLabel;

    // get buttons to restart game after stunned and return to main menu
    public Button restartGameBtn;
    public Button mainMenuBtn;
    public Button quitBtn;

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
        // prevent jump action if player is stunned
        jumpActive = jumpActive && !isStunned;

        // jump is only registered if the player isGrounded - prevents jump from being held down
        if (jumpActive && isGrounded)
        {
            // limit the amount of velocity added to the jump
            if(playerRigidBody.velocity.y <= 10)
            {
                playerRigidBody.AddForce(new Vector2(0, jumpForce));
            }
        }

        // if the player is not stunned then move forward
        if(!isStunned)
        {
            Vector2 newVelocity = playerRigidBody.velocity;
            newVelocity.x = forwardSpeed;
            playerRigidBody.velocity = newVelocity;

            // update yardsRun
            yardsRun += multiplier * Time.deltaTime;
            // update text and limit to one decimal point on display
            yardsRunLabel.text = yardsRun.ToString("F1");
            // Debug.Log(yardsRun);
        }

        // keep checking if player is grounded
        UpdateGroundedStatus();

        parallax.offset = transform.position.x;

        if(isStunned && isGrounded)
        {
            restartGameBtn.gameObject.SetActive(true);
            mainMenuBtn.gameObject.SetActive(true);
            quitBtn.gameObject.SetActive(true);
        }
    }

    void UpdateGroundedStatus()
    {
        // if the groundCheck object overlaps the ground layer then the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, 0.1f, groundCheckLayerMask);
        // this updates the animator isGrounded parameter
        playerAnimator.SetBool("isGrounded", isGrounded);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        HitByEnemy(collider);
    }

    void HitByEnemy(Collider2D enemyCollider)
    {
        isStunned = true;
        playerAnimator.SetBool("isStunned", true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
