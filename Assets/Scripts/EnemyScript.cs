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
    private float enemySpeed;

    public Sprite enemySprite;

    // Start is called before the first frame update
    void Start()
    {

        // set speed for each instantiated enemy object
        enemySpeed = Random.Range(enemyMinSpeed, enemyMaxSpeed); 
        //Debug.Log(enemySpeed);
        
    }

    // Update is called once per frame
    void Update()
    {
        // make enemy run towards player at random speed
        gameObject.transform.position += -transform.right * enemySpeed * Time.deltaTime;
    }
}