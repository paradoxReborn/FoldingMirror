using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Handles the entire main menu scene.
/// </summary>
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject ContinueButton;

    private void Start()
    {
        if (!LevelManager.LM.saveExists) ContinueButton.SetActive(false);
    }

    public void PlayTest()
    {
        //UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        LevelManager.LM.NewGame();
    }

    public void NewGame()
    {
        LevelManager.LM.NewGame();
    }

    public void ContinueGame()
    {
        LevelManager.LM.LoadGame();
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
