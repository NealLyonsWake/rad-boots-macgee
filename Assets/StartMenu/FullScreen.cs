using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreen : MonoBehaviour
{


    // public void FullScreening()
    //{
    // Toggle fullscreen
    //Screen.fullScreen = !Screen.fullScreen;
    //}

   void Start()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
