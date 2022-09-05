using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

    public float health = 2f;
    public int dieAngle = 90;

    public AudioSource enemyDeadSound;
    
    
    

    bool deadAlready = false;

    Animator animator;
    Collider m_collider;

    private void Start()
    {
        animator = GetComponent<Animator>();
        m_collider = GetComponent<Collider>();
    }



    void Update()
    {
        if (deadAlready && dieAngle >0)
        {

            dieAngle -= 2;
            transform.Rotate(-2,0,0);
            
        }
        if (deadAlready && dieAngle <= 0)
        {
            m_collider.enabled = false;
        }

    }



    public void TakeDamage(float amount)
    {
        PatrollingAI alert = GetComponent<PatrollingAI>();
        if (health > 0f)
        {
            health -= amount;
            animator.SetBool("Hit", true);

        }
       
        if(alert != null)
        {
            alert.SetAlert(true);
        }

        if(health <= 0f)
        {
            enemyDeadSound.Play();
            Die();
        }


    }

    void Die()
    {
        PatrollingAI patrolling = GetComponent<PatrollingAI>();
        if(patrolling != null)
        {
            patrolling.SetAlive(false);
            animator.SetBool("Dead", true);

        }
        
        StartCoroutine(Topple());
               
    }

    private IEnumerator Topple()
    {
        
        if (!deadAlready)
        {
            deadAlready = true;

        }
           
        yield return new WaitForSeconds(3f);

        Destroy(this.gameObject);
    }

   
}
