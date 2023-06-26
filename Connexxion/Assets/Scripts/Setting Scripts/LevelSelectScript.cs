using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectScript : MonoBehaviour
{
    public void MenuButton()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void LevelButton(int i)
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + i);
    }
}
