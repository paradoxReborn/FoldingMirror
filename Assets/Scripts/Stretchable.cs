using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stretchable : MonoBehaviour
{
    
    [SerializeField] private float ExtrudeFactor;

    private BoxCollider boxCollider;
    private DimensionController Controller;

    private Vector3 originalSize;
    private bool extruded;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();
        if (boxCollider == null) Debug.LogError(gameObject + "is missing its box collider.");
        originalSize = boxCollider.size;
        Controller = DimensionController.DC;
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
