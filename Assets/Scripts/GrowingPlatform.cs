using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Put this script on the trigger collider for the platform.
/// It will cause the platform to grow along its axis when the player touches the collider.
/// </summary>
public class GrowingPlatform : MonoBehaviour
{
    [SerializeField] private GameObject platform;
    [SerializeField] private float expansionSpeed;
    [SerializeField] private float maxScale;

    [SerializeField] private bool RequireSpecificAvatar = false;
    [SerializeField] private GameObject SpecificAvatar;

    private bool expanding;

    // Update is called once per frame
    void Update()
    {
        if (expanding && platform.transform.localScale.x < maxScale)
        {
            float x = expansionSpeed * Time.deltaTime;
            platform.transform.localScale += new Vector3(2*x, 0, 0);
            platform.transform.Translate(Vector3.right * x);
        }
        if (platform.transform.localScale.x >= maxScale) gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Platform triggered");
        if (!RequireSpecificAvatar && other.gameObject.GetComponent<MyCharacterController>() != null)
        {
            expanding = true;
            Debug.Log("Platform started expanding.");
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (!RequireSpecificAvatar && other.gameObject.GetComponent<MyCharacterController>() != null)
        {
            expanding = false;
            Debug.Log("Platform stopped expanding.");
        }

    }
}
