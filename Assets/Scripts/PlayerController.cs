using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Script that controls player movement, physics, colliders, audio and scores
    /// </summary>

    // force to apply to jump
    [SerializeField]
    private float jumpForce = 125.0f;
    [SerializeField]
    private float forwardSpeed = 3.5f;
    [SerializeField]
    private float maxSpeed = 7.5f;
    private float acceleration = 0.1f;

    // get player object and collider 
    private Rigidbody2D playerRigidBody;
    private BoxCollider2D playerCollider;

    // get audio source for player sounds and create serialize fields for sound clips
    private AudioSource playerAudio;
    [SerializeField]
    private AudioClip impact;
    [SerializeField]
    private AudioClip running;
    [SerializeField]
    private AudioClip jump;
    [SerializeField]
    private AudioClip pain;
    [SerializeField]
    private AudioClip skid;

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

    // variables to store stats
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

    // store player rigidbody position in previous frame
    private float lastPositionX;

    // Start is called before the first frame update
    void Start()
    {
        // get components for player elements that need to be edited during game
        playerAudio = GetComponent<AudioSource>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        // get half width for screen press recognition
        screenWidth = Screen.width / 2.0f;
        hiscore = PlayerPrefs.GetFloat("hiscore");

        if(PlayerPrefs.GetString("fxStatus") == "off")
        {
            AudioListener.volume = 0f;
        } else
        {
            AudioListener.volume = 1f;
        }
    }

    private void Update()
    {
        bool inputDetected = Input.GetButtonDown("Fire1");
        bool jumpDetected = Input.mousePosition.x < screenWidth;
        bool slideDetected = Input.mousePosition.x > screenWidth;

        // whilst the player is alive - increment counters
        if (!isStunned)
        {
            // if the screen is tapped
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

        // workaround for player position sticking - current x and y positions for frame
        float currentPositionX = playerRigidBody.transform.position.x;
        float currentPositionY = playerRigidBody.transform.position.y;

        // if current X is the same as last X - therefore stuck
        if (currentPositionX == lastPositionX)
        {
            // keep the x position the same but just bump the y to get over the collider edge
            playerRigidBody.position = new Vector2(currentPositionX, currentPositionY + 0.01f);
        }

        // record current x position as last x position after update
        lastPositionX = currentPositionX;
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
            playerAudio.Stop();
            if (!playerAudio.isPlaying)
            {
                playerAudio.PlayOneShot(jump, 0.5f);
            }


            // limit the amount of velocity added to the jump
            if(playerRigidBody.velocity.y <= 8)
            {
                playerRigidBody.AddForce(new Vector2(0, jumpForce));
            }
        }

        if (slideActive && isGrounded && !jumpActive)
        {
            if (!playerAudio.isPlaying)
            {
                playerAudio.Stop();
                playerAudio.PlayOneShot(skid, 0.5f);
            }

            // set size of collider whilst sliding
            playerAnimator.SetBool("isSliding", true);
            playerCollider.size = new Vector2(0.87f, 0.41f);
            playerCollider.offset = new Vector2(0f, -0.88f);
        } 
        else
        {
            playerAnimator.SetBool("isSliding", false);
        } 

        // if the player is not stunned then move forward
        if(!isStunned)
        {
            // play running sound 
            if(!playerAudio.isPlaying)
            {
                playerAudio.clip = running;
                playerAudio.Play();
            }

            // gradually increaase speed of player movement
            Vector2 newVelocity = playerRigidBody.velocity;
            if(forwardSpeed < maxSpeed)
            {
                forwardSpeed += acceleration * Time.deltaTime;
            }
            newVelocity.x = forwardSpeed;
            playerRigidBody.velocity = newVelocity;
            // Debug.Log(forwardSpeed);

            if (!slideActive)
            {
                // reset collider to starting size
                playerCollider.size = new Vector2(0.6f, 1.68f);
                playerCollider.offset = new Vector2(0f, -0.25f);
            }


            // update yardsRun
            yardsRun += multiplier * Time.deltaTime;
            // update text and zerofill 
            yardsRunLabel.text = yardsRun.ToString("00000");


        }

        // keep checking if player is grounded
        UpdateGroundedStatus();

        parallax.offset = transform.position.x;

        // if player is dead and on the ground
        if(isStunned && isGrounded)
        {
            // make buttons visible and set stats text from var values
            restartGameBtn.gameObject.SetActive(true);
            mainMenuBtn.gameObject.SetActive(true);
            statsPanel.gameObject.SetActive(true);
            maxYards.text = yardsRun.ToString("00000");
            jumpsText.text = jumps.ToString("00000");
            slidesText.text = slides.ToString("00000");
            
            // if hiscore was not beaten
            if(yardsRun <= hiscore)
            {
                bestYardsText.text = hiscore.ToString("00000");
                messageText.text = "*** TRY AGAIN ***";
            }
            else
            {
                bestYardsText.text = yardsRun.ToString("00000");
                messageText.text = "*** NEW HI SCORE ***";
                // only save score if it beats previous hiscore
                GetComponent<ScoreScript>().SaveScore(); 
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
        // change player status to stunned and play animation
        isStunned = true;
        playerAnimator.SetBool("isStunned", true);
        // stop any jump, skid or running sounds
        playerAudio.Stop();
        // play impact and pain sounds
        playerAudio.PlayOneShot(impact, 0.5f);
        playerAudio.PlayOneShot(pain, 0.5f);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
