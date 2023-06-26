using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UICanvasScript : MonoBehaviour
{
    public Sprite unmuteSprite;
    public Sprite muteSprite;

    private Image muteButtonSR;
    private GameObject panel;
    private bool paused;

    private void Start()
    {
        muteButtonSR = GameObject.Find("Mute Button").GetComponent<Image>();
        panel = GameObject.Find("UI Canvas/Pause Objects/Pause Panel");
        panel.SetActive(false);

        if (UniversalScript.instance.mute) muteButtonSR.sprite = muteSprite;
        else muteButtonSR.sprite = unmuteSprite;

        paused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused) PlayButton();
            else PauseButton();
        }
    }

    public void PauseButton()
    {
        if (!paused)
        {
            paused = true;
            panel.SetActive(true);
        }
    }

    public void PlayButton()
    {
        paused = false;
        panel.SetActive(false);
    }

    public void MuteButton()
    {
        UniversalScript.instance.MuteToggle(!UniversalScript.instance.mute);
        if (UniversalScript.instance.mute) muteButtonSR.sprite = muteSprite;
        else muteButtonSR.sprite = unmuteSprite;
    }

    public void QuitButton()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
