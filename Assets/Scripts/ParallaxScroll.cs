using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroll : MonoBehaviour
{
    /// <summary>
    /// script to set up parallax scrolling background in game scene
    /// </summary>

    // the parallax background layers
    public Renderer background;
    public Renderer midground;
    public Renderer foreground;

    // set scrolling speeds for each layer
    public float backgroundSpeed = 0.02f;
    public float midgroundSpeed = 0.04f;
    public float foregroundSpeed = 0.06f;

    // set by player position in PlayerController script
    public float offset = 0.0f;

    // Update is called once per frame
    void Update()
    {

        // this increases the layer textures offset making it move at different speeds
        float backgroundOffset = offset * backgroundSpeed;
        float foregroundOffset = offset * foregroundSpeed;
        float midgroundOffset = offset * midgroundSpeed;

        // move to new position
        background.material.mainTextureOffset = new Vector2(backgroundOffset, 0);
        foreground.material.mainTextureOffset = new Vector2(foregroundOffset, 0);
        midground.material.mainTextureOffset = new Vector2(midgroundOffset, 0);
        
    }
}
