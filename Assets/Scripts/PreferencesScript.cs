using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferencesScript : MonoBehaviour
{
    public void SetSoundFxPreference()
    {
        string toggle = PlayerPrefs.GetString("fxStatus");
        if (toggle == "off")
        {
            PlayerPrefs.SetString("fxStatus", "on");
        }
        else
        {
            PlayerPrefs.SetString("fxStatus", "off");
        }
        Debug.Log(PlayerPrefs.GetString("fxStatus"));
    }

    public void SetMusicPreference()
    {
        string toggle = PlayerPrefs.GetString("musicStatus");
        if(toggle == "off")
        {
            PlayerPrefs.SetString("musicStatus", "on");
        } else
        {
            PlayerPrefs.SetString("musicStatus", "off");
        }
        Debug.Log(PlayerPrefs.GetString("musicStatus"));
    }

    public void ResetHiscore()
    {
        PlayerPrefs.SetFloat("hiscore", 0f);
    }
}
