using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{

    /// <summary>
    /// script to control audio playing in menus, alter visual side of audio preferences and display hiscore
    /// </summary>

    // variables to store elements for displaying score and audio text toggle statuses
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

    private AudioSource menuAudio;

    // Start is called before the first frame update
    void Start()
    {
        // get audio source for playing clip
        menuAudio = GetComponent<AudioSource>();

        // start music if preference is on and reset volume
        if(PlayerPrefs.GetString("musicStatus") == "on")
        {
            menuAudio.Play();
            AudioListener.volume = 0.252f;
        }

        StartCoroutine(StatusChecks());
    }

    void UpdateHiscore()
    {
        // check for changes to hiscore and update if necessary
        ScoreText.text = PlayerPrefs.GetFloat("hiscore").ToString("00000");
    }

    void CheckSoundStatus()
    {
        string toggle = PlayerPrefs.GetString("fxStatus");


        if (toggle == "on")
        {
            // play fx and change toggle status
            soundOff.color = new Color32(77, 77, 77, 255); // grey
            soundOn.color = new Color32(255, 0, 0, 255); // red

        }
        else
        {
            // dont play fx and change toggle status
            soundOn.color = new Color32(77, 77, 77, 255); // grey
            soundOff.color = new Color32(255, 0, 0, 255); // red
            
        }
    }

    void CheckMusicStatus()
    {
        string toggle = PlayerPrefs.GetString("musicStatus");

        if (toggle == "on")
        {
            // play music and change toggle statu, set volume
            musicOff.color = new Color32(77, 77, 77, 255); // grey
            musicOn.color = new Color32(255, 0, 0, 255); // red
            menuAudio.volume = 0.252f;
            
        }
        else
        {
            // dont play fx and change toggle status, turn down volume
            musicOn.color = new Color32(77, 77, 77, 255); // grey
            musicOff.color = new Color32(255, 0, 0, 255); // red
            menuAudio.volume = 0f;

        }
    }

    // set up for constantly checking changes to audio preferences or hiscore
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
