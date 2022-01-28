using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// GameStateManager handles all gamestates and switches between them.
/// Objects that work only while in a specific state should check GameStateManager's flags.
/// </summary>
public class GameStateManager : MonoBehaviour
{
    [SerializeField] KeyCode pauseKey = KeyCode.Escape;
    [SerializeField] GameObject pauseMenu;

    public bool playing { get; private set; }
    public bool gameOver { get; private set; } //This may not be used as failstates aren't currently planned.
    public bool levelComplete { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        playing = true;
        gameOver = false;
        levelComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(levelComplete || gameOver)
        {
            playing = false;
        }

        if (Input.GetKeyDown(pauseKey) && !gameOver && !levelComplete)
        {
            if (playing) //Toggle pause
            {
                playing = false;
                pauseMenu.SetActive(true);
            }
            else
            {
                pauseMenu.SetActive(false);
                playing = true;
            }
        }
    }

    // Restart current level - called by UI
    public void UIRestartLevel()
    {
        string currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
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
