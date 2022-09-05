using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            Invoke(nameof(Hide), 0.25f);
        }
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }



}
