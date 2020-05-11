using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{

    /// <summary>
    /// setter function for hiscore, value is set from PlayerController script
    /// </summary>

    // player object
    [SerializeField]
    private GameObject player;

    public void SaveScore()
    {
        // set hiscore value in player preferences
        PlayerPrefs.SetFloat("hiscore", player.GetComponent<PlayerController>().YardsRun);
    }

}
