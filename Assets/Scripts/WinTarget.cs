using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A pressure plate or button that sets a win condition true if a specific Character touches it.
//Object must have a WinCondition component.
public class WinTarget : MonoBehaviour
{
    [SerializeField] private string TargetCharacter;
    [SerializeField] private bool StayTrue;
    [SerializeField] private bool HideOnTrigger;
    [SerializeField] private bool RotateModel;
    [SerializeField] private float RotateSpeed;
    
    private WinCondition condition;

    void Start()
    {
        condition = gameObject.GetComponent<WinCondition>();
    }

    void Update()
    {
        if (RotateModel)
        {
            transform.Rotate(Vector3.up * RotateSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<MyCharacterController>().CharacterID == TargetCharacter)
        {
            condition.win = true;
            if(HideOnTrigger) gameObject.GetComponent<Renderer>().enabled = false;
        }
        //TODO Check character and character name
        //Debug.Log(other + " hit me.");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!StayTrue && other.gameObject.GetComponent<MyCharacterController>().CharacterID == TargetCharacter)
        {
            condition.win = false;
            if(HideOnTrigger) gameObject.GetComponent<Renderer>().enabled = true;
        }
    }
}
