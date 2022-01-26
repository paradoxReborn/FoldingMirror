using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlipper : MonoBehaviour
{
    [SerializeField] DimensionController DC;
    [SerializeField] Vector3 _2DViewPosition;
    [SerializeField] Vector3 _2DViewRotation;
    [SerializeField] Vector3 _3DViewPosition;
    [SerializeField] Vector3 _3DViewRotation;

    private Quaternion SplitViewQrt;
    private Quaternion CombinedViewQrt;

    private bool split;

    // Start is called before the first frame update
    void Start()
    {
        SplitViewQrt = Quaternion.Euler(_2DViewRotation);
        CombinedViewQrt = Quaternion.Euler(_3DViewRotation);

        //Initial camera positioning
        if (DC.StartSplit) Show2DView();
        else Show3DView();
    }

    // Update is called once per frame
    void Update()
    {
        if(split != DC.split)
        {
            if (split) //switch to combined view
            {
                Show3DView();
                split = false;
            }
            else //switch to split view
            {
                Show2DView();
                split = true;
            }
        }
    }

    private void Show3DView()
    {
        transform.position = _3DViewPosition;
        transform.rotation = CombinedViewQrt;
    }

    private void Show2DView()
    {
        transform.position = _2DViewPosition;
        transform.rotation = SplitViewQrt;
    }
}
