using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{
    public void PlayTest()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
#if UNITY_STANDALONE
        Application.Quit();
#endif
    }
}
