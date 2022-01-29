using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A persistent singleton that handles progress in the game.
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager LM { get; private set; }
    public int levelProgress { get; private set; } = 0; // Equals the last level the player has finished.
    public int currentLevel { get; private set; } = 0;

    private string savePath;

    private void Awake()
    {
        // Singleton
        if (LM != null) Destroy(LM);
        LM = this;

        // Setup
        savePath = Application.persistentDataPath + "/save";
        currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

        // Load progress
        if (System.IO.File.Exists(savePath))
            levelProgress = JsonUtility.FromJson<int>(System.IO.File.ReadAllText(savePath));
        else levelProgress = 0;
    }

    // Clear old savegame and load level 1
    public void NewGame()
    {
        System.IO.File.WriteAllText(savePath, JsonUtility.ToJson(0));
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    // Load the next level
    public void LoadGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelProgress+1);
    }

    // Continue to the next level
    public void NextLevel()
    {
        // Sanity check for level complete
        if (GameStateManager.GM.levelComplete)
        {
            //Save progress and load next scene
            System.IO.File.WriteAllText(savePath, JsonUtility.ToJson(currentLevel));
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentLevel+1);
        }
    }

    // Restart current level
    public void RestartLevel()
    {
        int currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
    }
}
