using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    // Set sprites for the two action states that enemy can have
    // public Sprite enemyJump;
    // public Sprite enemySlide;

    public float enemyMaxMovementSpeed = 2.0f;
    public float enemyJumpForce = 60.0f;
    
    public float actionInterval = 0.5f;
    private bool isJumping = false;
    private bool isSliding = false;
    private float timeUntilNextAction;

    private Collider2D enemyCollider;
    //private SpriteRenderer enemyRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
