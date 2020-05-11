using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitOnClick : MonoBehaviour
{
    /// <summary>
    /// Script to exit application, attached to quit buttons
    /// </summary>
    
    public void ExitGame()
    {
        Application.Quit();
    }
}
