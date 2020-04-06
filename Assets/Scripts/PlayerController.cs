using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // force to apply to jump
    [SerializeField]
    private float jumpForce = 80.0f;
    [SerializeField]
    private float forwardSpeed = 3.0f;

    private Rigidbody2D playerRigidBody;
    private BoxCollider2D playerCollider;

    // variables for groundCheck for animation transition
    // this will store a reference to the groundCheck child object
    public Transform groundCheckTransform;
    private bool isGrounded;
    // this will store the Layer which is defined as ground (floor)
    public LayerMask groundCheckLayerMask;
    // this will be a reference to the animator component
    private Animator playerAnimator;

    // bool var for stunned status
    private bool isStunned = false;

    // scrolling background
    public ParallaxScroll parallax;

    // variables to track and display score
    [SerializeField]
    private float yardsRun = 0;
    [SerializeField]
    private float multiplier = 2.0f;
    public Text yardsRunLabel;

    private uint jumps = 0;
    private uint slides = 0;

    // score property so that score can be passed to ScoreScript for storage
    public float YardsRun 
    { 
        get { return yardsRun; }
    }

    // get hiscore to display at end of game
    public float hiscore;

    // get buttons to restart game after stunned and return to main menu
    public Button restartGameBtn;
    public Button mainMenuBtn;
    public Button quitBtn;
    public Image statsPanel;
    public Text maxYards;
    public Text bestYardsText;
    public Text slidesText;
    public Text jumpsText;
    public Text messageText;

    // screen width for input check
    private float screenWidth;
    // see if jump action was made
    private bool jumpActive; 
    // see if slide action was made
    bool slideActive; 

    // Start is called before the first frame update
    void Start()
    {

        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        screenWidth = Screen.width / 2.0f;
        hiscore = PlayerPrefs.GetFloat("hiscore");

    }

    private void Update()
    {
        bool inputDetected = Input.GetButtonDown("Fire1");
        bool jumpDetected = Input.mousePosition.x < screenWidth;
        bool slideDetected = Input.mousePosition.x > screenWidth;

        if (inputDetected)
        {
            if (jumpDetected)
            {
                jumps++;
            } 
            else if (slideDetected)
            {
                slides++;
            }
        }
    }

    void FixedUpdate()
    {
        bool inputDetected = Input.GetButton("Fire1");

        // if left half of screen is pressed
        jumpActive = Input.mousePosition.x < screenWidth && inputDetected && !isStunned;
        // if right half of screen is pressed
        slideActive = Input.mousePosition.x > screenWidth && inputDetected && !isStunned;

        // jump is only registered if the player isGrounded - prevents jump from being held down
        if (jumpActive && isGrounded)
        {
            // limit the amount of velocity added to the jump
            if(playerRigidBody.velocity.y <= 8)
            {
                playerRigidBody.AddForce(new Vector2(0, jumpForce));
            }
        }

        if (slideActive && isGrounded)
        {
            playerAnimator.SetBool("isSliding", true);
            // playerCollider.size = new Vector2(0.87f, 0.41f);
            // playerCollider.offset = new Vector2(0f, -1.15f);
            // Debug.Log("Sliding");
            // Debug.Log(playerCollider.size);
            // Debug.Log(playerCollider.offset);

        } 
        else
        {
            playerAnimator.SetBool("isSliding", false);
        } 

        // if the player is not stunned then move forward
        if(!isStunned)
        {
            Vector2 newVelocity = playerRigidBody.velocity;
            newVelocity.x = forwardSpeed;
            playerRigidBody.velocity = newVelocity;

           // playerCollider.size = new Vector2(0.6f, 1.68f);
           // playerCollider.offset = new Vector2(0f, 0.2f);
           // Debug.Log("Running/Jumping");
            //Debug.Log(playerCollider.size);
           // Debug.Log(playerCollider.offset);

            // update yardsRun
            yardsRun += multiplier * Time.deltaTime;
            // update text and zerofill 
            yardsRunLabel.text = yardsRun.ToString("00000");

        }

        // keep checking if player is grounded
        UpdateGroundedStatus();

        parallax.offset = transform.position.x;

        if(isStunned && isGrounded)
        {
            GetComponent<ScoreScript>().SaveScore();
            restartGameBtn.gameObject.SetActive(true);
            mainMenuBtn.gameObject.SetActive(true);
            statsPanel.gameObject.SetActive(true);
            quitBtn.gameObject.SetActive(true);
            maxYards.text = yardsRun.ToString("00000");
            jumpsText.text = jumps.ToString("00000");
            slidesText.text = slides.ToString("00000");
            
            if(yardsRun <= hiscore)
            {
                bestYardsText.text = hiscore.ToString("00000");
                messageText.text = "*** TRY AGAIN ***";
            }
            else
            {
                bestYardsText.text = yardsRun.ToString("00000");
                messageText.text = "*** NEW HI SCORE ***";
            }
            
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
