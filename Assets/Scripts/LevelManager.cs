using System;
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
    public bool saveExists { get; private set; }

    private SaveGame saveGame;

    private string savePath;

    private void Awake()
    {
        // Singleton
        if (LM != null) Destroy(LM);
        LM = this;

        // Setup
        savePath = Application.persistentDataPath + "/save";
        Debug.Log("Savegame is located at: " + savePath);
        currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        if (System.IO.File.Exists(savePath))
        {
            saveExists = true;
            saveGame = JsonUtility.FromJson<SaveGame>(System.IO.File.ReadAllText(savePath));
            levelProgress = saveGame.LevelCompleted;
            Debug.Log("Loaded level progress. Completed level: " + levelProgress);
        }
        else
        {
            saveExists = false;
            levelProgress = 0;
        }
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
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelProgress + 1);
    }

    // Continue to the next level
    public void NextLevel()
    {
        // Sanity check for level complete
        if (GameStateManager.GM.levelComplete)
        {
            //Save progress and load next scene
            saveGame.LevelCompleted = currentLevel;
            System.IO.File.WriteAllText(savePath, JsonUtility.ToJson(saveGame));
            Debug.Log("Saved progress. Completed level " + saveGame.LevelCompleted);
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentLevel + 1);
        }
    }

    // Restart current level
    public void RestartLevel()
    {
        int currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene);
    }

    [Serializable]
    private struct SaveGame{
        public int LevelCompleted;
    }
}
