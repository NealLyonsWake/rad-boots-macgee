using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnClones : MonoBehaviour
{
    public GameObject clone;

    private void Start()
    {

        InvokeRepeating("Clonings", 2f, 2f);

    }
            

    void Clonings()
    {
        Instantiate(clone, transform.position, clone.transform.rotation);
    }



}
