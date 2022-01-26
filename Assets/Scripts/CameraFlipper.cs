using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlipper : MonoBehaviour
{
    [SerializeField] DimensionController DC;
    [SerializeField] Vector3 SplitViewPosition;
    [SerializeField] Vector3 SplitViewRotation;
    [SerializeField] Vector3 _3DViewPosition;
    [SerializeField] Vector3 _3DViewRotation;

    private Quaternion SplitViewQrt;
    private Quaternion CombinedViewQrt;

    private bool split;

    // Start is called before the first frame update
    void Start()
    {
        SplitViewQrt = Quaternion.Euler(SplitViewRotation);
        CombinedViewQrt = Quaternion.Euler(_3DViewRotation);
    }

    // Update is called once per frame
    void Update()
    {
        if(split != DC.split)
        {
            if (split) //switch to combined view
            {
                transform.position = _3DViewPosition;
                transform.rotation = CombinedViewQrt;
                split = false;
            }
            else //switch to split view
            {
                transform.position = SplitViewPosition;
                transform.rotation = SplitViewQrt;
                split = true;
            }
        }
    }

}
