using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Game";

    public void GameQuit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
