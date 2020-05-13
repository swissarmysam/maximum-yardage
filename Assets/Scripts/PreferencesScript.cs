using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferencesScript : MonoBehaviour
{

    /// <summary>
    /// Script to set player preferences for audio and also allow hiscore to be reset - attached to buttons in settings screen in main menu scene
    /// </summary>
   
    public void SetSoundFxPreference()
    {
        // get player prefs key
        string toggle = PlayerPrefs.GetString("fxStatus");

        // toggle between on and off
        if (toggle == "off")
        {
            PlayerPrefs.SetString("fxStatus", "on");
        }
        else
        {
            PlayerPrefs.SetString("fxStatus", "off");
        }
    }

    public void SetMusicPreference()
    {
        // get player prefs key
        string toggle = PlayerPrefs.GetString("musicStatus");

        // toggle between on and off
        if(toggle == "off")
        {
            PlayerPrefs.SetString("musicStatus", "on");
        } 
        else
        {
            PlayerPrefs.SetString("musicStatus", "off");
        }
    }

    public void ResetHiscore()
    {
        // set the hiscore to 0
        PlayerPrefs.SetFloat("hiscore", 0f);
    }
}
