using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is essentially a wrapper around Unity's built-in CharacterController.
/// Charactercontroller is used because it allows collision detection without Rigidbody physics.
/// 
/// Each of the characters in the game contains a MyCharacterController.
/// The doppelganger is placed upside-down, and thus behaves as a mirror image.
/// </summary>

public class MyCharacterController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.3f;
    [SerializeField] private float playerGravity = 0.04f;
    [SerializeField] private float groundedRadius = 1.1f;
    [SerializeField] private float JumpStrength = 0.22f;
    [SerializeField] private float JumpFalloff = 0.06f;
    [SerializeField] private float JumpHang = 0.02f;
    [SerializeField] private int CoyoteFrames = 2;
    [SerializeField] private bool _3D = true;
    [SerializeField] private string CharacterName; // To distinguish between the different avatars

    public string CharacterID { get; private set; }

    private CharacterController ctrl;
    private GameStateManager GM;

    public bool grounded {get; private set;}
    private bool canJump;
    private bool jumping;
    private int remCoyote;
    private float remJump;
    private Vector3 playerVelocity;

    // Start is called before the first frame update
    void Start()
    {
        GM = GameStateManager.GM;
        ctrl = gameObject.GetComponent<CharacterController>();
        CharacterID = CharacterName;

        if (ctrl == null) Debug.LogError("Character missing charactercontroller: " + gameObject.name);
        jumping = false;
    }

    // Read jump key
    void Update()
    {
        if (GM.playing)
        {
            if (Input.GetKeyDown(KeyCode.Space) && canJump)
            {
                Debug.Log("jump!");
                remJump = JumpStrength;
                jumping = true;
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                jumping = false;
            }
        }
    }

    // Do physics
    private void FixedUpdate()
    {
        if (GM.playing)
        {
            grounded = Grounded();// only need one cast per physics frame

            //if grounded, reset jump and coyote time. If not, apply gravity acceleration
            if (grounded)
            {
                canJump = true;
                remCoyote = CoyoteFrames;
                Debug.Log("Character grounded");
                playerVelocity.y = -playerGravity;
            }
            else if (remCoyote < 1) playerVelocity.y -= playerGravity;
            else remCoyote--;

            // calculate vertical movement
            if (jumping)
            {
                canJump = false; //ensure we can't start another jump until grounded and button released
                playerVelocity.y += remJump;
                remJump = Mathf.Max(remJump -= JumpFalloff, JumpHang);
                Debug.Log("Jumping");
            }

            // calculate side movement
            playerVelocity.x = Input.GetAxis("Horizontal") * moveSpeed;

            //if in 3D, do forward movement
            if (_3D)
            {
                playerVelocity.z = Input.GetAxis("Vertical") * moveSpeed;
            }

            // do calculated movement
            ctrl.Move(gameObject.transform.rotation * playerVelocity);
        }
    }

    // Replacement for unreliable or non-working CharacterController collision detection
    private bool Grounded()
    {
        if (Physics.Raycast(transform.position, gameObject.transform.rotation * Vector3.down, groundedRadius, ~0, QueryTriggerInteraction.Ignore)) return true;
        return false;
    }
}
