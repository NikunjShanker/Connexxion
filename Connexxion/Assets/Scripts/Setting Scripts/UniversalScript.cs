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

    public void MuteSounds()
    {
        mute = true;
    }

    public void UnmuteSounds()
    {
        mute = false;
    }
}
