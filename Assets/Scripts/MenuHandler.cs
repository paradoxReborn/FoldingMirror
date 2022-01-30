using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Handle visibility of buttons and provide functionality to the same.
/// </summary>
public class MenuHandler : MonoBehaviour
{ 
    [SerializeField] private GameObject NextLevelButton;
    [SerializeField] private GameObject RestartLevelButton;
    [SerializeField] private GameObject QuitGameButton;
    [SerializeField] private GameObject ContinueText;
    [SerializeField] private Text MenuLabel;

    private void OnEnable()
    {
        if (GameStateManager.GM.levelComplete)
        {
            NextLevelButton.SetActive(true);
            MenuLabel.text = "Level Complete";
        }
        else
        {
            NextLevelButton.SetActive(false);
            if (GameStateManager.GM.gameOver) MenuLabel.text = "OOF!";
            else MenuLabel.text = "Menu";
        }

        if (!GameStateManager.GM.levelComplete && !GameStateManager.GM.gameOver) ContinueText.SetActive(true);
        else ContinueText.SetActive(false);
    }

    // Continue to the next level
    public void UINextLevel()
    {
        LevelManager.LM.NextLevel();
        /*
        // Sanity check for level complete
        if (GameStateManager.GM.levelComplete)
        {
            //TODO load NEXT level instead of reloading this one.
            Debug.LogWarning("Load Next Level requested. This is not yet implemented.");
            string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
        }
        */
    }

    // Restart current level - called by UI
    public void UIRestartLevel()
    {
        LevelManager.LM.RestartLevel();
        /*
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
        */
    }

    // Quit game - called by UI
    public void UIQuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
#if UNITY_STANDALONE
        Application.Quit();
#endif
    }
}
