using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelectionManager : MonoBehaviour
{

    public void LoadMainMenu()
    {
        // in case returning after pause
        Time.timeScale = 1f;
        ScoreKeeper.ResetScore();

        SceneManager.LoadScene("MainMenu");
    }

    public void StartFirstLevel()
    {
        LoadLevel(1);
    }

    public void LoadLevel(int levelNumber)
    {
        SceneManager.LoadScene($"Level_{levelNumber}");
    }

    public void LoadNextLevel()
    {
        // in case returning after pause
        Time.timeScale = 1f;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex;

        // If they completed the level (at least one star) load next level
        if (ScoreKeeper.OneStar)
            nextSceneIndex++;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
            nextSceneIndex = 0;

        SceneManager.LoadScene(nextSceneIndex);
    }

    public void LoadControls()
    {
        SceneManager.LoadScene("Controls");
    }

    public void LoadSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }
    
    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}