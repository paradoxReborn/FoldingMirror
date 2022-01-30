using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A configurable switch trigger that can also act as a win condition.
/// 
/// </summary>
public class Switch : MonoBehaviour
{
    //[SerializeField] Material OnMaterial;
    //[SerializeField] Material OffMaterial;
    [SerializeField] private GameObject OnPrefab;
    [SerializeField] private GameObject OffPrefab;
    [SerializeField] private Vector3 PrefabOffset;
    [SerializeField] private Vector3 PrefabRotation;

    [SerializeField] bool startOn = false;
    [SerializeField] bool toggle = false;
    [SerializeField] bool momentary = false;
    [SerializeField] bool invisibleWhenOn = false;

    [SerializeField] bool winCondition = false;

    [SerializeField] bool RequireSpecificAvatar = false;
    [SerializeField] GameObject SpecificAvatar;

    private Quaternion PrefabQuaternion;
    private Renderer switchRenderer;
    private GameObject switchGraphic;
    private WinCondition myWinCond;
    public bool on { get; private set; }

    void turnOff()
    {
        on = false;
        if (invisibleWhenOn) switchRenderer.enabled = true;
        else ChangeGraphic(OffPrefab);
        if (winCondition) myWinCond.win = false;
    }

    void turnOn()
    {
        on = true;
        if (invisibleWhenOn) switchRenderer.enabled = false;
        else ChangeGraphic(OnPrefab);
        if (winCondition) myWinCond.win = true;
    }

    private void Awake()
    {
        if (winCondition) myWinCond = gameObject.AddComponent(typeof(WinCondition)) as WinCondition;
    }

    void Start()
    {
        PrefabQuaternion = Quaternion.Euler(PrefabRotation);

        if (startOn) ChangeGraphic(OnPrefab);
        else ChangeGraphic(OffPrefab);

        if (startOn) turnOn();
        else turnOff();

        // Sanity check
        if (toggle) momentary = false;
    }

    // Switch to a new graphic if there is one
    void ChangeGraphic(GameObject newGraphic)
    {
        if (switchGraphic != null) Destroy(switchGraphic);
        if (newGraphic != null)
        {
            switchGraphic = Instantiate(newGraphic, gameObject.transform.position + PrefabOffset, PrefabQuaternion, gameObject.transform);
            switchRenderer = switchGraphic.GetComponent<Renderer>();
        }
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
