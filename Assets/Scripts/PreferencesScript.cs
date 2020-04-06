using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferencesScript : MonoBehaviour
{

    [SerializeField]
    private GameObject player;

    public void SaveSoundFxPreference()
    {
        PlayerPrefs.SetString("toggle", "on");
    }

    public void SaveMusicPreference()
    {
        PlayerPrefs.SetString("toggle", "on");
    }

    public void ResetHiscore()
    {
        PlayerPrefs.SetFloat("hiscore", 0f);
    }
}
