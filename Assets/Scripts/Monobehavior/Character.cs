using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
public class Character : MonoBehaviour
{
    [SerializeField] private float moveSpeed; //character's max horizontal speed.
    //[SerializeField] private float rampSpeed; //how much speed the character gains each fixedUpdate
    //[SerializeField] private float stopSpeed; //how fast the character comes to a halt

    [SerializeField] private float jumpForce; //initial "launch" of the jump
    [SerializeField] private float jumpFalloff; //how quickly the "launch" fades
    [SerializeField] private float jumpHang; //minimum force applied while holding jump key
    [SerializeField] private float characterGravity; //gravitational acceleration applied to character

    private CharacterController controller;
    private bool flat = false; //true when the character is split and working in 2D

    private bool grounded;
    private bool jumping;
    private bool canJump;
    private float currentJump;
    private Vector3 characterVelocity;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        currentJump = jumpForce;
        characterVelocity = Vector3.zero;
        grounded = false;
        jumping = false;
        canJump = false;
    }


    void Update()
    {

    }

    // Controls
    void FixedUpdate()
    {
        controller.Move(Vector3.down * characterGravity);
        bool groundedNow = grounded || controller.isGrounded;

        characterVelocity.x = Input.GetAxis("Horizontal") * moveSpeed;

        if (groundedNow) characterVelocity.y = -characterGravity;
        else characterVelocity.y -= characterGravity;

        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            //Start jump
            jumping = true;
            canJump = false;
            currentJump = jumpForce;
            Debug.Log("Jumping with force: " + currentJump);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            //End jump
            jumping = false;
        }
        
        // Jump and hang
        if (Input.GetKey(KeyCode.Space) && jumping)
        {
            //Hang
            characterVelocity.y += Mathf.Max(jumpHang, currentJump);
            currentJump -= jumpFalloff;
            //Debug.Log("velocity set to: " + characterVelocity);
        }

        //do movement
        controller.Move(characterVelocity);

        grounded = controller.isGrounded;
    }
}
