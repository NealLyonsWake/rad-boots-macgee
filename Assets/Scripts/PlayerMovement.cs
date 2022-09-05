using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 6; // move speed
    public float lerpSpeed = 10; // smoothing speed
    private float gravity = 10; // gravity acceleration
    private bool isGrounded;
    private float deltaGround = 0.2f; // character is grounded up to this distance
    public float jumpSpeed = 10; // vertical jump initial speed
    public float jumpRange = 4; // range to detect target wall
    private Vector3 surfaceNormal; // current surface normal
    private Vector3 myNormal; // character normal
    private float distGround; // distance from character position to ground
    private bool jumping = false; // flag &quot;I'm jumping to wall&quot;

    private Transform myTransform;
    public BoxCollider boxCollider; // drag BoxCollider ref in editor
    public Rigidbody rigidBody;
    public LayerMask bootStick; // Ignore this layer for wall jumping
    public bool playerAlive = true;
    public bool _playerReady = false;
    private bool firstFlip = false;

    public Transform glitchProtector;
    

    private void Start()
    {
        myNormal = transform.up; // normal starts as character up direction
        myTransform = transform;
        rigidBody.freezeRotation = true; // disable physics rotation
                                         // distance from transform.position to ground
        distGround = boxCollider.size.y - boxCollider.center.y;

    }

    private void FixedUpdate()
    {
        // apply constant weight force according to character normal:
        rigidBody.AddForce(-gravity * rigidBody.mass * myNormal);
    }

    private void Update()
    {
        // jump code - jump to wall or simple jump
        if (jumping) return; // abort Update while jumping to a wall

        Ray ray;
        RaycastHit hit;

        if (Input.GetButtonDown("Jump") && playerAlive)
        { // jump pressed:
            ray = new Ray(myTransform.position, myTransform.forward);
            if (Physics.Raycast(ray, out hit, jumpRange, bootStick) && _playerReady)
            { // wall ahead?
                JumpToWall(hit.point, hit.normal); // yes: jump to the wall
                if (!firstFlip)
                {
                    Dialogue nextInstruction = myTransform.GetComponent<Dialogue>();
                    nextInstruction.FlipTrigger();
                }
            }
            else if (isGrounded)
            { // no: if grounded, jump up
                rigidBody.velocity += jumpSpeed * myNormal;
            }
        }

        // movement code - turn left/right with Horizontal axis:
        if (playerAlive)
        {
            myTransform.Translate(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, 0, 0);
        }
        // update surface normal and isGrounded:
        ray = new Ray(myTransform.position, -myNormal); // cast ray downwards
        if (Physics.Raycast(ray, out hit))
        { // use it to update myNormal and isGrounded
            isGrounded = hit.distance <= distGround + deltaGround;
            surfaceNormal = hit.normal;
        }
        else
        {
            isGrounded = false;
            // assume usual ground normal to avoid "falling forever"
            surfaceNormal = Vector3.up;
        }
        myNormal = Vector3.Lerp(myNormal, surfaceNormal, lerpSpeed * Time.deltaTime);
        // find forward direction with new myNormal:
        Vector3 myForward = Vector3.Cross(myTransform.right, myNormal);
        // align character to the new myNormal while keeping the forward direction:
        Quaternion targetRot = Quaternion.LookRotation(myForward, myNormal);
        myTransform.rotation = Quaternion.Lerp(myTransform.rotation, targetRot, lerpSpeed * Time.deltaTime);
        // move the character forth/back with Vertical axis:
        if (playerAlive)
        {
            myTransform.Translate(0, 0, Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);
        }

        
        // Check if player has fell out of level

        if(myTransform.position.y < -23)
        {
            myTransform.position = glitchProtector.position;
        }

        if (myTransform.position.y > 30)
        {
            myTransform.position = glitchProtector.position;
        }

        if (myTransform.position.z < -30)
        {
            myTransform.position = glitchProtector.position;
        }

        if (myTransform.position.z > 50)
        {
            myTransform.position = glitchProtector.position;
        }

        if (myTransform.position.x < -60)
        {
            myTransform.position = glitchProtector.position;
        }

        if(myTransform.position.x > 40)
        {
            myTransform.position = glitchProtector.position;
        }


    }

    private void JumpToWall(Vector3 point, Vector3 normal)
    {
        // jump to wall
        jumping = true; // signal it's jumping to wall
        rigidBody.isKinematic = true; // disable physics while jumping
        Vector3 orgPos = myTransform.position;
        Quaternion orgRot = myTransform.rotation;
        Vector3 dstPos = point + normal * (distGround + 0.5f); // will jump to 0.5 above wall
        Vector3 myForward = Vector3.Cross(myTransform.right, normal);
        Quaternion dstRot = Quaternion.LookRotation(myForward, normal);

        StartCoroutine(jumpTime(orgPos, orgRot, dstPos, dstRot, normal));
        //jumptime
    }

    private IEnumerator jumpTime(Vector3 orgPos, Quaternion orgRot, Vector3 dstPos, Quaternion dstRot, Vector3 normal)
    {
        for (float t = 0.0f; t < 1.0f;)
        {
            t += Time.deltaTime;
            myTransform.position = Vector3.Lerp(orgPos, dstPos, t);
            myTransform.rotation = Quaternion.Slerp(orgRot, dstRot, t);
            yield return null; // return here next frame
        }
        myNormal = normal; // update myNormal
        rigidBody.isKinematic = false; // enable physics
        jumping = false; // jumping to wall finished

    }

    public void PlayerDead(bool isAlive)
    {
        playerAlive = isAlive;
        
    }

    public void PlayerReady()
    {
        _playerReady = true;
    }


}
