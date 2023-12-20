using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController playerController;
    
    // Ground
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private LayerMask GroundMask;
    [SerializeField] private float GroundDistance;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float jumpStrength = 10f;
    private bool isGrounded;
    //private readonly float MAX_GRAVITY = -100f;
    
    // Input movement storing
    private Vector3 HorizontalVector;
    private Vector3 VerticalVector;
    private bool _jumped;

    void Start() {
        playerController = gameObject.GetComponent<CharacterController>();
        if (playerController == null) Debug.Log("NO PLAYER"); 
    }
    void Update()
    {   // REMEMBER TO ALWAYS CHECK IF COLLIDING OBJECTS ARE OF LAYER: GROUND
        
        // Get input for horizontal movement
        Vector3 moveX = Input.GetAxis("Horizontal") * transform.right;
        Vector3 moveZ = Input.GetAxis("Vertical") * transform.forward;

        HorizontalVector = speed * Time.deltaTime * (moveX + moveZ);

        // Get input for jumping
        if (Input.GetButtonDown("Jump")) {
            _jumped = true;
        }

        HorizontalMove();
        VerticalMove();
    }

    bool CheckGround() {
        isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);
        return isGrounded;
    }

    private void VerticalMove()
    {
        // Player on the ground
        if (CheckGround() && VerticalVector.y < 0) {
            VerticalVector.y = -2f;
        }
        // Jumping
        if (_jumped) {
            _jumped = false;
            VerticalVector.y = Mathf.Sqrt(jumpStrength * -2f * gravity);
        }
        VerticalVector.y += gravity * Time.deltaTime;
        
        // deltaTime squared because of Freefall Formula
        playerController.Move(VerticalVector * Time.deltaTime);
    }

    private void HorizontalMove()
    {
        playerController.Move(HorizontalVector);
    }
}
