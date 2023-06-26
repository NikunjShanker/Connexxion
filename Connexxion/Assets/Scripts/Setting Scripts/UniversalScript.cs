using UnityEngine;
using UnityEngine.SceneManagement;

public class UniversalScript : MonoBehaviour
{
    public static UniversalScript instance;

    public bool mute;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    public void completeLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex < 20)
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void MuteToggle(bool m)
    {
        mute = m;
    }
}
