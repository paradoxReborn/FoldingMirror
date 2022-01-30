using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class catanimate3D : MonoBehaviour
{
    Animator animator;
    // MyCharacterController myCharacter = MyCharacterController();
    [SerializeField] MyCharacterController charcontroller;
    
    private Vector3 playerVelocity;
    CharacterController controller;
    float xinput;
    float zinput;

    public float rotationspeed;
    Vector3 movedir;

    // Start is called before the first frame update
    void Start()
    {
          animator = GetComponent<Animator>();
        charcontroller = gameObject.GetComponent<MyCharacterController>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        xinput = Input.GetAxisRaw("Horizontal");
        zinput = Input.GetAxisRaw("Vertical");
        movedir = new Vector3(xinput,0,zinput);
        movedir.Normalize();
        
        if (movedir != Vector3.zero)
        {
            // transform.Rotate(0,Input.GetAxis("Rotate")*60*Time.deltaTime,0);
            // gameObject.transform.rotation = movedir;
            Quaternion toRotation = Quaternion.LookRotation(movedir, Vector3.up);
            gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, toRotation, rotationspeed * Time.deltaTime);
        }

         animator.SetBool("walking",false);
        if ((Input.GetAxisRaw("Horizontal") !=0) || (Input.GetAxisRaw("Vertical") !=0))
        {
            animator.SetBool("walking",true);
        }
     
        animator.SetBool("grounded",charcontroller.grounded);

    }
}
