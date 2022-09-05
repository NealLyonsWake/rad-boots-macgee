using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{


    public Transform player;
    public AudioSource audioSource;
    public AudioClip[] audioClipArray;
    static int clipIndex = 0; // Changed from private to static
    private bool walkTrigger = false;
    private bool lookTrigger = false;

    private float triggerZ = -18;
    private float topRotation = 0.95f;
    private float bottomRotation = -0.95f;

    private bool firstFlip = false;
    private bool finalClip = false;
    private float endXAndY = -20;

    static bool showSpeech = true; // Changed to static
    private bool replay = false;

    // Dialogue
    public GameObject dialogue1;
    public GameObject dialogue2;
    public GameObject dialogue3;
    public GameObject dialogue4;
    public GameObject dialogue5;
    public GameObject dialogue6;
    public GameObject dialogue7;
    public GameObject dialogue8;
    public GameObject dialogue9;
    public GameObject dialogue10;
    public GameObject dialogue11;

    // Keys
    public GameObject wasd;
    public GameObject space;


    // Start is called before the first frame update
    void Start()
    {
        if (clipIndex == 0)
        {
            audioSource.clip = audioClipArray[clipIndex];
        }
    }
        
    

    // Update is called once per frame
    void Update()
    {
        if (!showSpeech && !replay)
        {
            PlayerMovement flipReady = player.GetComponent<PlayerMovement>();
            flipReady.PlayerReady();
            replay = true;
        }

        if (showSpeech && audioSource.clip == audioClipArray[clipIndex] && !audioSource.isPlaying && clipIndex <10)
        {

            audioSource.Play();

            // Show speech
            if (showSpeech && clipIndex == 0)
            {
                dialogue1.SetActive(true);
            }
            if (showSpeech && clipIndex == 1)
            {
                dialogue1.SetActive(false);
                dialogue2.SetActive(true);
            }
            if (showSpeech && clipIndex == 2)
            {
                dialogue2.SetActive(false);
                dialogue3.SetActive(true);
            }
            if (showSpeech && clipIndex == 3)
            {
                dialogue3.SetActive(false);
                dialogue4.SetActive(true);
            }
            if (showSpeech && clipIndex == 4)
            {
                dialogue4.SetActive(false);
                dialogue5.SetActive(true);
                wasd.SetActive(true);
            }
            if (showSpeech && clipIndex == 5)
            {
                dialogue5.SetActive(false);
                dialogue6.SetActive(true);
                wasd.SetActive(true);
            }
            if (showSpeech && clipIndex == 6)
            {
                dialogue6.SetActive(false);
                dialogue7.SetActive(true);
                wasd.SetActive(false);
                
            }
            if (showSpeech && clipIndex == 7)
            {
                dialogue7.SetActive(false);
                dialogue8.SetActive(true);
                space.SetActive(true);
            }
            if (showSpeech && clipIndex == 8)
            {
                dialogue8.SetActive(false);
                dialogue9.SetActive(true);
                space.SetActive(false);
            }
            if (showSpeech && clipIndex == 9)
            {
                dialogue9.SetActive(false);
                dialogue10.SetActive(true);
            }

            clipIndex += 1;

            


        }
        else
        {
            SwitchAudio();
        }

        if(!audioSource.isPlaying && showSpeech && clipIndex == 10)
        {
            dialogue10.SetActive(false);
            showSpeech = false;
        }


        if (audioSource.clip == audioClipArray[clipIndex] && !audioSource.isPlaying && clipIndex == 10 && !finalClip)
        {

            audioSource.Play();
            finalClip = true;
            dialogue11.SetActive(true);
            Invoke(nameof(NextScene), 13f); 
        }

        if (clipIndex == 6 && player.transform.position.z > triggerZ)
        {
            walkTrigger = true;
            
        }

        if (clipIndex == 6 && player.rotation.y > topRotation)
        {
            lookTrigger = true;
            
        }
        if (clipIndex == 6 && player.rotation.y < bottomRotation)
        {
            lookTrigger = true;
            
        }

        if(player.transform.position.x < endXAndY && player.transform.position.y < endXAndY && clipIndex ==10)
        {
            audioSource.clip = audioClipArray[clipIndex];
        }

        



    }

    void SwitchAudio()
    {
        if (!audioSource.isPlaying && clipIndex < 6)
        {
            audioSource.clip = audioClipArray[clipIndex];
        }

        if (clipIndex == 6 && walkTrigger && lookTrigger)
        {
            audioSource.clip = audioClipArray[clipIndex];

            PlayerMovement flipReady = player.GetComponent<PlayerMovement>();
            flipReady.PlayerReady();

        }

        if (!audioSource.isPlaying && clipIndex > 6 && clipIndex < 8)
        {
            audioSource.clip = audioClipArray[clipIndex];
        }

        if (!audioSource.isPlaying && clipIndex == 8 && firstFlip)
        {
            audioSource.clip = audioClipArray[clipIndex];
        }

        if (!audioSource.isPlaying && clipIndex > 8 && clipIndex < 10)
        {
            audioSource.clip = audioClipArray[clipIndex];
        }

       


    }

    public void FlipTrigger()
    {
        firstFlip = true;
    }


    void NextScene()
    {
        clipIndex = 0;
        showSpeech = true;
        SceneManager.LoadScene("End");
    }

}