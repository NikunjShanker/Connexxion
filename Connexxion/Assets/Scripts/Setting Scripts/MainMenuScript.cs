using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void LevelButton()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void CreditsButton()
    {
        SceneManager.LoadSceneAsync(20);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
