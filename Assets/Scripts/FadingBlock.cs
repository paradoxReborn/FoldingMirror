using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingBlock : MonoBehaviour
{
    [SerializeField] private GameObject SwitchObject;

    [SerializeField] private Material SolidMaterial;
    [SerializeField] private Material GhostMaterial;
    [SerializeField] private bool startSolid = false;

    private Switch mySwitch;
    private BoxCollider boxCollider;
    private Renderer thisRenderer;

    void GoSolid()
    {
        boxCollider.enabled = true;
        thisRenderer.material = SolidMaterial;
    }

    void GoGhost()
    {
        boxCollider.enabled = false;
        thisRenderer.material = GhostMaterial;
    }

    // Start is called before the first frame update
    void Start()
    {
        mySwitch = SwitchObject.GetComponent<Switch>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
        thisRenderer = gameObject.GetComponent<Renderer>();

        if (startSolid) GoSolid();
        else GoGhost();
    }

    // Update is called once per frame
    void Update()
    {
        if (mySwitch.on)
        {
            GoSolid();
        }
        else
        {
            GoGhost();
        }
    }
}
