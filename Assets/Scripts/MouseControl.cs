using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 100f;
    [SerializeField] private float mouseSensitivity = 10f;
    [SerializeField] private Transform playerBody;
    private float mouseY;
    private float mouseX;
    private float xRotation = 0f;
    private float yRotation = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // For some reason you have to -= add to vertical movement
        mouseY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;

        xRotation = Mathf.Lerp(xRotation, mouseY, cameraSpeed * Time.deltaTime);
        yRotation = Mathf.Lerp(yRotation, mouseX, cameraSpeed * Time.deltaTime);
        
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        MoveMouse();
    }

    void MoveMouse() {
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        //transform.Rotate(xRotation, 0f, 0f);
        playerBody.Rotate(0f, yRotation, 0f);
    }
}
