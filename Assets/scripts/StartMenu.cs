using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    // Load the main game scene
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // Quit the game application
    public void QuitGame()
    {
        UnityEngine.Application.Quit();
    }
}
