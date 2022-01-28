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
    [SerializeField] GameObject pauseMenu;
    [SerializeField] KeyCode pauseKey = KeyCode.Escape;
    [SerializeField] int nextLevelIndex;
    [SerializeField] GameObject[] WinConditions;

    private WinCondition[] Win;

    public bool playing { get; private set; }
    public bool gameOver { get; private set; } //This may not be used as failstates aren't currently planned.
    public bool levelComplete { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        playing = true;
        gameOver = false;
        levelComplete = false;

        // Cache win condition components
        Win = new WinCondition[WinConditions.Length];
        //Debug.Log("Winconditions: " + WinConditions.Length);
        for(int i = 0; i < WinConditions.Length; i++)
        {
            Win[i] = WinConditions[i].GetComponent<WinCondition>();
            //Debug.Log("Win condition " + i + ": " + WinConditions[i] + ", component registered: " + Win[i]);
            if (Win[i] == null) Debug.LogError(gameObject + " is missing its WinCondition component.");
        }
        //Debug.Log("GameStateManager Started.");
    }

    // Update is called once per frame
    void Update()
    {
        // Set levelComplete, clear if any win condition is not true.
        levelComplete = true;
        for(int i = 0; i < WinConditions.Length; i++)
            if (!Win[i].win) levelComplete = false;

        // Stop the game if the level is finished or failed.
        if (levelComplete || gameOver)
        {
            playing = false;
            pauseMenu.SetActive(true);
            Debug.Log("Level finished.");
        }

        if (Input.GetKeyDown(pauseKey) && !gameOver && !levelComplete)
        {
            if (playing) //Toggle pause
            {

                playing = false;
                pauseMenu.SetActive(true);
                Debug.Log("Paused.");
            }
            else
            {
                pauseMenu.SetActive(false);
                playing = true;
                Debug.Log("Unpaused");
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
