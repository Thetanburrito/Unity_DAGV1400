using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Initializing all the 'public' variables needed for player movement and control
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = 9.81f;

    //Initicalizing all the private variables needed for player movement and control
    private CharacterController controller;
    private Vector3 moveDirection;
    private bool isJumping;

    //I am adding a few that wont be used til later, for animation and what not
    private bool isFalling;
    private bool isGrounded;
    private bool isCrouched;
    private bool isSprinting;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
