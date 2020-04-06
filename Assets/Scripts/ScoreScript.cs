using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{

    [SerializeField]
    private GameObject player;

    public void SaveScore()
    {
        PlayerPrefs.SetFloat("hiscore", player.GetComponent<PlayerController>().YardsRun);
    }

}
