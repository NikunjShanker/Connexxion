using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour
{
    public void ContinueButton()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
