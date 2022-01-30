using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameStateManager handles all gamestates and switches between them.
/// Objects that work only while in a specific state should check GameStateManager's flags.
/// </summary>
public class GameStateManager : MonoBehaviour
{
    //Singleton Instance
    public static GameStateManager GM { get; private set; }

    [SerializeField] GameObject pauseMenu;
    [SerializeField] KeyCode pauseKey = KeyCode.Escape;
    //[SerializeField] int nextLevelIndex;
    [SerializeField] WinCondition[] WinConditions;

    //private WinCondition[] Win;

    public bool playing { get; private set; }
    public bool gameOver { get; private set; } //This may not be used as failstates aren't currently planned.
    public bool levelComplete { get; private set; }

    // Set/Enforce singleton
    private void Awake()
    {
        if (GM != null) Destroy(GM);
        GM = this;
    }

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
        // Set levelComplete if all win conditions are true.
        bool finish = true;
        for(int i = 0; i < WinConditions.Length; i++)
            //if (!Win[i].win) finish = false;
            if (!WinConditions[i].win) finish = false;
        levelComplete = finish;

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
}
