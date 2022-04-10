using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelectionManager : MonoBehaviour
{

    [SerializeField] float sceneLoadDelay = 2f;
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
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex;

        // If they completed the level (at least one star) load next level
        if (ScoreKeeper.OneStar)
            nextSceneIndex++;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
            nextSceneIndex = 0;

        StartCoroutine(WaitAndLoadBySceneIndex(nextSceneIndex, sceneLoadDelay));
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

    IEnumerator WaitAndLoadBySceneName(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator WaitAndLoadBySceneIndex(int sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneIndex);
    }
}