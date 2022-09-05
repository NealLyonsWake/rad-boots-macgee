using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 15;
    public int currentHealth;
    public HealthBar healthBar;
    public PlayerMovement lockMove;
    public GameObject playerHit;
    public AudioSource playerHitSound;
    
    
    public bool isAlive = true;
    public Camera camHealth;
    public Gun _gun;
    


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }


    public void PlayerShot(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (isAlive)
        {
            playerHit.SetActive(true);
            playerHitSound.Play();
        }


        if (currentHealth <=0)
        {
            isAlive = false;
            lockMove.PlayerDead(isAlive);
            MouseLook mouseLock = camHealth.GetComponent<MouseLook>();
            mouseLock.MouseLock(isAlive);
            Gun gun = _gun.GetComponent<Gun>();
            gun.GunLock(isAlive);

        }

    }



}
