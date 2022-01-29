using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Roll credits, then return to main menu.
/// </summary>
public class CreditScroll : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 100;
    [SerializeField] private float returnToMenuAfter = 60;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * scrollSpeed);
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(returnToMenuAfter);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
