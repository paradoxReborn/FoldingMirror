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
    [SerializeField] bool invisibleWhenOn = false;

    [SerializeField] bool winCondition = false;

    [SerializeField] bool RequireSpecificAvatar = false;
    [SerializeField] GameObject SpecificAvatar;

    private Renderer thisRenderer;
    private WinCondition myWinCond;
    public bool on { get; private set; }

    void turnOff()
    {
        on = false;
        if (invisibleWhenOn) thisRenderer.enabled = true;
        else thisRenderer.material = OffMaterial;
        if (winCondition) myWinCond.win = false;
    }

    void turnOn()
    {
        on = true;
        if (invisibleWhenOn) thisRenderer.enabled = false;
        else thisRenderer.material = OnMaterial;
        if (winCondition) myWinCond.win = true;
    }

    private void Awake()
    {
        if (winCondition) myWinCond = gameObject.AddComponent(typeof(WinCondition)) as WinCondition;
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
        // Make sure it's a player character. If set for a specific avatar, make sure it's that one.
        if(other.gameObject.GetComponent<MyCharacterController>() != null)
        {
            if(!RequireSpecificAvatar || GameObject.ReferenceEquals(other.gameObject, SpecificAvatar))
            {
                if (toggle && on) turnOff();
                else turnOn();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(momentary && other.gameObject.GetComponent<MyCharacterController>() != null)
        {
            if (!RequireSpecificAvatar || GameObject.ReferenceEquals(other.gameObject, SpecificAvatar))
            {
                turnOff();
            }
        }
    }
}
