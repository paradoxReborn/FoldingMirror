using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlippableObstacle : MonoBehaviour
{
    [SerializeField] private OldDimensionController DC;
    [SerializeField] private float ExtrudeFactor = 50f;

    private BoxCollider boxCollider;

    private Vector3 originalSize;
    private bool extruded;

    // Start is called before the first frame update
    void Start()
    {
        if(DC == null)
        {
            DC = GameObject.Find("DimensionController").GetComponent<OldDimensionController>(); //In case someone forgets to assign this.
        }
        boxCollider = gameObject.GetComponent<BoxCollider>();
        originalSize = boxCollider.size;
        if (DC.StartSplit)
        {
            ExtrudeForm();
            extruded = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(extruded != DC.split)
        {
            if (extruded)
            {
                ResetForm();
                extruded = false;
            }
            else
            {
                ExtrudeForm();
                extruded = true;
            }
        }
    }

    private void ExtrudeForm()
    {
        //transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z * 50f);
        boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y, boxCollider.size.z * ExtrudeFactor);
    }

    private void ResetForm()
    {
        //transform.localScale = originalSize;
        boxCollider.size = originalSize;
    }
}
