using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 1500f;
    public Transform playerBody;
    private bool playerAlive = true;
    private float deadCamRotation = 90f;

    private float rotationX = 0f;
    private float minX = -90f;
    private float maxX = 90f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAlive)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, minX, maxX);

            transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
            playerBody.Rotate(Vector3.up, mouseX);
        }

        if (!playerAlive && deadCamRotation >=0f)
        {
            transform.localRotation = Quaternion.Euler(-1, 0, 0);
            deadCamRotation -= 1 * Time.deltaTime;
        }


    }

    public void MouseLock(bool isAlive)
    {
        playerAlive = isAlive;
        Cursor.lockState = CursorLockMode.None;
    }




}
