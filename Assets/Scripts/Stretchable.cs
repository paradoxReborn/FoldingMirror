using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stretchable : MonoBehaviour
{
    [SerializeField] private DimensionController Controller;
    [SerializeField] private float ExtrudeFactor;

    private BoxCollider boxCollider;

    private Vector3 originalSize;
    private bool extruded;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
        if (boxCollider == null) Debug.LogError(gameObject + "'s Stretchable component must have a reference to the Dimension Controller. Set one in the inspector.");
        originalSize = boxCollider.size;
    }

    // Update is called once per frame
    void Update()
    {
        if(extruded != Controller.ExtendColliderFlag)
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
