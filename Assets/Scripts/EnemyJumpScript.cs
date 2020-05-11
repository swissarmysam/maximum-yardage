using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumpScript : MonoBehaviour
{
    // set upper and lower limit for enemy running speed
    public float enemyMinSpeed = 2.0f;
    public float enemyMaxSpeed = 6.0f;
    private float enemySpeed;

    // set upper and lower limit for jump height
    public float enemyMinJumpForce = 75.0f;
    public float enemyMaxJumpForce = 200.0f;
    private float enemyJump;

    // set upper and lower values for jumping action, create var for interval until next action
    private float actionInterval;
    public float actionMinInterval = 0.25f;
    public float actionMaxInterval = 0.75f;
    private float timeUntilNextAction;

    // flag variable used to control jump state and animator
    private bool isJumping = true;

    // set componenets to control velocity/force and animator state
    private Rigidbody2D enemyRigidBody;
    private Animator enemyAnimator;
    private BoxCollider2D enemyCollider;


    // Start is called before the first frame update
    void Start()
    {
        // get componenets to control velocity/force and animator state
        enemyRigidBody = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        enemyCollider = GetComponent<BoxCollider2D>();

        // set random interval for jumping action 
        actionInterval = Random.Range(actionMinInterval, actionMaxInterval);
        timeUntilNextAction = actionInterval;

        // set speed and jump force for each instantiated enemy object
        enemySpeed = Random.Range(enemyMinSpeed, enemyMaxSpeed);
        enemyJump = Random.Range(enemyMinJumpForce, enemyMaxJumpForce);

    }

    // Update is called once per frame
    void Update()
    {

        // count down time until next action
        timeUntilNextAction -= Time.deltaTime;

        // when timer runs out
        if(timeUntilNextAction <= 0)
        {
            // change jumping state
            isJumping = !isJumping;

            if (isJumping)
            {
                // change jumping animation and add jumping force
                enemyAnimator.SetBool("isJumping", true);
                enemyRigidBody.AddForce(new Vector2(0F, enemyJump));

                // redefine collider for jump animation here
                enemyCollider.offset = new Vector2(0.15f, 0.26f);
                enemyCollider.size = new Vector2(0.6f, 0.6f);
            }
            else
            {
                // change back to running animation
                enemyAnimator.SetBool("isJumping", false);
                enemyRigidBody.velocity = new Vector2(0f, 0f);

                // redefine collider for running animation here
                enemyCollider.offset = new Vector2(0.15f, 0f);
                enemyCollider.size = new Vector2(0.6f, 1.6f);
            }
            // reset timer
            timeUntilNextAction = actionInterval;
        }

        // make enemy run towards player at random speed
       enemyRigidBody.velocity = new Vector2(-enemySpeed, 0f);

    }
}