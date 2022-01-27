using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingEnvironment : MonoBehaviour
{
    [SerializeField] private float unfoldedX = 0;
    [SerializeField] private float foldawayX = -160;
    [SerializeField] private int transitionFrames = 60;
    [SerializeField] private bool startOpen;

    private float deltaAngle;
    private float targetX;

    public void Fold()
    {
        targetX = foldawayX;
    }

    public void Unfold()
    {
        targetX = unfoldedX;
    }

    // Start is called before the first frame update
    void Start()
    {
        deltaAngle = Mathf.Abs((unfoldedX - foldawayX) / (float)transitionFrames);
        if (startOpen) targetX = unfoldedX;
        else targetX = foldawayX;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetX,transform.rotation.y,transform.rotation.z), deltaAngle);
    }
}
