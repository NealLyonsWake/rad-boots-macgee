using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public float damage = 1f;
    public float range = 25f;
    public ParticleSystem playerMuzzleFlash;
    public GameObject impactEffect;
    public float fireRate =15f;
    private float nextTimeToFire = 0f;
    private bool gunActive = true;
    public AudioSource audioSource;
    [SerializeField] private GameObject impactindicator;
    
    public Camera fpsCam;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1")&& Time.time >= nextTimeToFire && gunActive)
        {
            playerMuzzleFlash.Play();
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            audioSource.Play();
            
        }
        
        

    }

    void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            
            Target target = hit.transform.GetComponent<Target>();
            if(target != null)
            {
                target.TakeDamage(damage);
                

            }

            Instantiate(impactindicator, hit.point, Quaternion.LookRotation(hit.normal));
            GameObject impactGO = Instantiate(impactEffect,hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO,0.5f);



        }

                
    }
    public void GunLock(bool isAlive)
    {
        gunActive = isAlive;
    }

}
