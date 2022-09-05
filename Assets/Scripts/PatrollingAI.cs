using UnityEngine;

public class PatrollingAI : MonoBehaviour
{
    public float speed; // Walking speed of enemy
    public float siteRange; // Site range for reacting to objects
    public float periperalVision; // Peripheral vision - how far the enemy can see things side to side
    

    public Transform player; // Grab player position in code
    public Transform gunImpact; // Grab the position of the player gun impact
    public LayerMask playerMask; // Grab layer assigned to player
    public Transform enemyGun; // Grab transform of gun
    Animator animator; // Grab animation state machine of enemy
    public ParticleSystem muzzleFlash; // Grab particle system of enemy gun
    public AudioSource audiosource; 

    // Grab enemy bullet info
    [SerializeField] private GameObject bulletPrefab;
    private GameObject _bullet;


    public float timeBetweenAttacks; // Cool-off time between enemy shots
    bool alreadyAttacked; // Switch for attacking mode of player
    public float attackRange; // Range of enemy sight of shooting
    bool attackPlayer; // Switch to attack player
    bool _alive; // Check state if enemy is alive
    bool turning = false; // Check state if player is turning 180 degrees
    
    bool _alerted; // Switch for enemy alerted procedure
    bool hit = false; // Enemy to stop if hit state

    // Animation events for enemy shooting recoil
    public AnimationCurve recoilCurve;
    public float recoilDuration = 0.25f;
    public float maxRotation = 45f;
    public Transform leftLowerArm;
    public Transform leftHand;
    private float recoilTimer;

    


    private void Start()
    {
        _alive = true; // Enemy is alive at start
        _alerted = false; // Enemy is not alerted of player at start
        attackPlayer = false; // Set bool to false when starting up
        animator = GetComponent<Animator>(); // Grab the animator component of enemy
        Debug.Log("");
        
        
    }

    public void SetAlive(bool alive) // Called from Target script to check if enemy has died
    {
        _alive = alive;
    }

    public void SetAlert(bool alerted) // Called from Target to check if enemy has been hit
    {
        _alerted = alerted;
        hit = true;
    }


    void Update()
    {
      
        attackPlayer = Physics.CheckSphere(transform.position, attackRange, playerMask); // Declare the bool if player is or isn't in range

        if (!attackPlayer && _alive && !_alerted && !turning && !hit) Patrolling();
        if (attackPlayer && _alive && !hit)
        {
            _alerted = true;
            turning = false;
            Attack();
        }

        if (!attackPlayer && _alive && _alerted && !turning && !hit) Alerted();
        if (!attackPlayer && turning && !_alerted && _alive && !hit) Turning();
        if (hit && _alive) EnemyHit();

        if (!_alive)
        {
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsAlertedWalking", false);
        }

        

    }


    void OnAnimatorIK()
    {

        if (_alive && _alerted && !hit) // Check if enemy is alerted and alive
        {
            // Aim gun at player
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, player.position);


            // Look at player
            animator.SetLookAtWeight(1);
            animator.SetLookAtPosition(player.position);
        }

    }

    void EnemyHit()
    {
        Invoke(nameof(ResetEnemy), 1f);
    }

    void ResetEnemy()
    {
        hit = false;
        animator.SetBool("Hit", false);
        turning = false;

    }

    void Patrolling()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // Walking enemy
        animator.SetBool("IsWalking", true);

        Ray ray = new Ray(transform.position, transform.forward); // Look forwards for obstacles
        RaycastHit hit;
        if (Physics.SphereCast(ray, periperalVision, out hit))
        {
            if (hit.distance < siteRange)
            {
                turning = true;
                animator.SetBool("Hit", false);
            }
        }
    }

    void Alerted()
    {
        Vector3 facePlayer = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(facePlayer);

        attackRange = 9;
        speed = 3;

        Ray ray = new Ray(transform.position, transform.forward); // Look forwards for obstacles
        RaycastHit hit;
        if (Physics.SphereCast(ray, periperalVision, out hit))
        {
            if (hit.distance < siteRange)
            {
                transform.Translate(Vector3.forward * 0);
                animator.SetBool("IsAlertedWalking", false);
                animator.SetBool("IsWalking", false);
                animator.SetBool("Hit", false);
            }
            else
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime); // Walking enemy
                animator.SetBool("IsAlertedWalking", true);
                animator.SetBool("IsWalking", false);
                animator.SetBool("Hit", false);
            }
        }
        turning = false;


    }



    void Turning()
    {

        transform.rotation = Quaternion.AngleAxis(180, Vector3.up) * transform.rotation;
        turning = false;

        
    }

    void Attack()
    {
        Vector3 facePlayer = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        transform.LookAt(facePlayer);


        animator.SetBool("IsWalking", false);
        animator.SetBool("IsAlertedWalking", false);
        animator.SetBool("Hit", false);


        if (!alreadyAttacked)
        {
            RaycastHit searchHit;
            if (Physics.Raycast(enemyGun.transform.position, enemyGun.transform.forward, out searchHit, attackRange))
            {
                

                PlayerHealth playerHealth = searchHit.transform.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    
                    // Attack Code here
                    muzzleFlash.Play();
                    audiosource.Play();
                    _bullet = Instantiate(bulletPrefab) as GameObject;
                    _bullet.transform.position = enemyGun.transform.TransformPoint(Vector3.forward * 0.2f);
                    _bullet.transform.rotation = enemyGun.rotation;
                    

                    recoilTimer = Time.time;
                    

                    alreadyAttacked = true;
                    Invoke(nameof(ResetAttack), timeBetweenAttacks);
                    // Attack Code End

                }

            }                
            
        }
    }


    void ResetAttack()
    {
        alreadyAttacked = false;
    }


    private void LateUpdate()
    {
        if (recoilTimer < 0)
        {
            return;
        }

        float curveTime = (Time.time - recoilTimer) / recoilDuration;
        if (curveTime > 1f)
        {
            recoilTimer = -1;
        }
        else
        {
            leftLowerArm.Rotate(Vector3.right, recoilCurve.Evaluate(curveTime) * maxRotation, Space.Self);
        }


    }

}
