using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    // Set sprites for the two action states that enemy can have
    // public Sprite enemyJump;
    // public Sprite enemySlide;

    public float enemyMinSpeed = 2.0f;
    public float enemyMaxSpeed = 6.0f;
    public float enemySpeed;
    public float enemyJumpForce = 20.0f;
    
    public float actionInterval = 0.5f;
    private bool isJumping = false;
    private bool isSliding = false;
    private float timeUntilNextAction;

    public Sprite enemySprite;
    private Collider2D enemyCollider;
    private SpriteRenderer enemyRenderer;

    // Start is called before the first frame update
    void Start()
    {

        // references to collider and renderer so adjustments can be made to properties whilst active
        enemyCollider = gameObject.GetComponent<Collider2D>();
        enemyRenderer = gameObject.GetComponent<SpriteRenderer>();
        // set speed for each instantiated enemy object
        enemySpeed = Random.Range(enemyMinSpeed, enemyMaxSpeed); 
        Debug.Log(enemySpeed);
        
    }

    // Update is called once per frame
    void Update()
    {
        // make enemy run towards player at random speed
        gameObject.transform.position += -transform.right * enemySpeed * Time.deltaTime;
    }
}