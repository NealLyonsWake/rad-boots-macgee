﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(EnditAll), 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnditAll() 
    {
        SceneManager.LoadScene("StartMenu");
    }


}
