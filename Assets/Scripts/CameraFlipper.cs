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

    [SerializeField] private float smoothTranslationSpeed = 0.5f;
    [SerializeField] private float smoothRotationSpeed = 1f;

    private Quaternion SplitViewQrt;
    private Quaternion CombinedViewQrt;

    private Quaternion targetRotation;
    private Vector3 targetPosition;

    //private Vector3 smoothVelocity = Vector3.zero;
    public bool split { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        SplitViewQrt = Quaternion.Euler(_2DViewRotation);
        CombinedViewQrt = Quaternion.Euler(_3DViewRotation);

        split = DC.StartSplit;

        //Initial camera positioning
        if (DC.StartSplit)
        {
            Transition2D();
        }
        else
        {
            Transition3D();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Control moved to DimensionController
        /*
        if(split != DC.split)
        {
            if (split) //switch to combined view
            {
                //Show3DView();
                targetPosition = _3DViewPosition;
                targetRotation = CombinedViewQrt;
                split = false;
            }
            else //switch to split view
            {
                //Show2DView();
                targetPosition = _2DViewPosition;
                targetRotation = SplitViewQrt;
                split = true;
            }
        }
        */

        //Follow target camera location smoothly
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, smoothRotationSpeed);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, smoothTranslationSpeed);
    }
    
    public void Transition3D()
    {
        targetPosition = _3DViewPosition;
        targetRotation = CombinedViewQrt;
        split = false;
    }

    public void Transition2D()
    {
        targetPosition = _2DViewPosition;
        targetRotation = SplitViewQrt;
        split = true;
    }

    /*
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
    */
}
