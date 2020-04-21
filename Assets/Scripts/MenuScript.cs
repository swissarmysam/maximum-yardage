using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{

    [SerializeField]
    private Text ScoreText;
    [SerializeField]
    private Text soundOff;
    [SerializeField]
    private Text soundOn;
    [SerializeField]
    private Text musicOff;
    [SerializeField]
    private Text musicOn;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StatusChecks());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateHiscore()
    {
        ScoreText.text = PlayerPrefs.GetFloat("hiscore").ToString("00000");
    }

    void CheckSoundStatus()
    {
        string toggle = PlayerPrefs.GetString("fxStatus");


        if (toggle == "on")
        {
            soundOff.color = new Color32(77, 77, 77, 255);
            soundOn.color = new Color32(255, 0, 0, 255);

            // play fx
        }
        else
        {
            // dont play fx
            soundOn.color = new Color32(77, 77, 77, 255);
            soundOff.color = new Color32(255, 0, 0, 255);
        }
    }

    void CheckMusicStatus()
    {
        string toggle = PlayerPrefs.GetString("musicStatus");

        if (toggle == "on")
        {
            musicOff.color = new Color32(77, 77, 77, 255);
            musicOn.color = new Color32(255, 0, 0, 255);

            // play fx
        }
        else
        {
            // dont play fx
            musicOn.color = new Color32(77, 77, 77, 255);
            musicOff.color = new Color32(255, 0, 0, 255);
        }
    }

    private IEnumerator StatusChecks()
    {
        while (true)
        {
            CheckMusicStatus();
            CheckSoundStatus();
            UpdateHiscore();
            // add pause before executing status check again
            yield return new WaitForSeconds(1f);
        }
    }
}
