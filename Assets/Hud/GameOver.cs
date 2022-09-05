using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public GameObject reticle;

    public void DisplayGameOver()
    {
        gameObject.SetActive(true);
        reticle.SetActive(false);
    }


}
