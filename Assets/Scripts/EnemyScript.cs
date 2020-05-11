using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    /// <summary>
    /// EnemyScript is attached to enemy prefab which is then instantiated by GeneratorScript
    /// </summary>

    // set upper and lower constraints for running speed
    public float enemyMinSpeed = 2.0f;
    public float enemyMaxSpeed = 6.0f;
    private float enemySpeed;

    // var for enemy sprite
    public Sprite enemySprite;

    // Start is called before the first frame update
    void Start()
    {
        // set speed for each instantiated enemy object
        enemySpeed = Random.Range(enemyMinSpeed, enemyMaxSpeed); 
    }

    // Update is called once per frame
    void Update()
    {
        // make enemy run towards player at random speed
        gameObject.transform.position += -transform.right * enemySpeed * Time.deltaTime;
    }
}