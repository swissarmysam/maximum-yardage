using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{

    /// <summary>
    /// Scene handler for main menu and game scenes
    /// </summary>
    /// <param name="sceneName"></param>
   
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
