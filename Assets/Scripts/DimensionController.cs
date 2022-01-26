using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This object controls the state of the avatars and mirror stage.
public class DimensionController : MonoBehaviour
{
    [SerializeField] private KeyCode FlipKey = KeyCode.F;
    [SerializeField] private GameObject CombinedAvatar;
    [SerializeField] private GameObject LightAvatar;
    [SerializeField] private GameObject DarkAvatar;
    [SerializeField] public bool StartSplit { get; private set; } = false;

    public bool split { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        split = StartSplit;
        //Ensure light/dark/combined avatars start in the right dimension
        CombinedAvatar.SetActive(!StartSplit);
        LightAvatar.SetActive(StartSplit);
        DarkAvatar.SetActive(StartSplit);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(FlipKey))
        {
            Debug.Log("Flip commanded");
            if (split) UnSplit();
            else Split();
        }
    }

    private void Split()
    {
        split = true;
        LightAvatar.transform.position = CombinedAvatar.transform.position;
        DarkAvatar.transform.position = new Vector3(
            CombinedAvatar.transform.position.x,
            -CombinedAvatar.transform.position.y,
            CombinedAvatar.transform.position.z);
        CombinedAvatar.SetActive(false);
        LightAvatar.SetActive(true);
        DarkAvatar.SetActive(true);

    }

    private void UnSplit()
    {
        split = false;
        CombinedAvatar.transform.position = LightAvatar.transform.position;
        CombinedAvatar.SetActive(true);
        LightAvatar.SetActive(false);
        DarkAvatar.SetActive(false);
    }
}
