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
        if (Input.GetAxisRaw("Horizontal") == -1)
        {
            flipcheck = true;
            animator.SetBool("walking",true);
        }
        else if (Input.GetAxisRaw("Horizontal") == 1)
        {
            flipcheck = false;
            animator.SetBool("walking",true);
        }
        animator.SetBool("grounded",charcontroller.grounded);

        m_spriterender.flipX = false;
        if (flipcheck == true )
        {
            m_spriterender.flipX = true;
        }
    }
}
