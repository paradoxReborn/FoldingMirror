using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] Material OnMaterial;
    [SerializeField] Material OffMaterial;
    [SerializeField] bool startOn = false;
    [SerializeField] bool toggle = false;
    [SerializeField] bool momentary = false;

    private Renderer thisRenderer;
    public bool on { get; private set; }

    void turnOff()
    {
        on = false;
        thisRenderer.material = OffMaterial;
    }

    void turnOn()
    {
        on = true;
        thisRenderer.material = OnMaterial;
    }

    void Start()
    {
        thisRenderer = gameObject.GetComponent<Renderer>();

        if (startOn) turnOn();
        else turnOff();

        // Sanity check
        if (toggle) momentary = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Make sure it's a player character.
        if(other.gameObject.GetComponent<MyCharacterController>() != null)
        {
            if (toggle && on) turnOff();
            else turnOn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(momentary && other.gameObject.GetComponent<MyCharacterController>() != null)
        {
            turnOff();
        }
    }
}
