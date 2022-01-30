using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catanimate : MonoBehaviour
{
    Animator animator;
    SpriteRenderer m_spriterender;
    // MyCharacterController myCharacter = MyCharacterController();
    [SerializeField] MyCharacterController charcontroller;
    

    private bool flipcheck;
    private Vector3 playerVelocity;
    CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        m_spriterender = GetComponent<SpriteRenderer>();
        charcontroller = gameObject.GetComponent<MyCharacterController>();
        controller = GetComponent<CharacterController>();
        m_spriterender.transform.Translate(0,1,0);
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("walking",false);
        if (Input.GetAxis("Horizontal") < -0.05)
        {
            flipcheck = true;
            animator.SetBool("walking",true);
        }
        else if (Input.GetAxis("Horizontal") > 0.05)
        {
            flipcheck = false;
            animator.SetBool("walking",true);
        }
        animator.SetInteger("jumping",0);
        animator.SetBool("grounded",true);
        if (charcontroller.grounded == false)
        {
            animator.SetBool("grounded",false);
            if (controller.velocity.y>0)
            {
                animator.SetInteger("jumping",1);
            }
            else
            {
                animator.SetInteger("jumping",-1);
            }
        }

        m_spriterender.flipX = false;
        if (flipcheck == true )
        {
            m_spriterender.flipX = true;
        }
    }
}
