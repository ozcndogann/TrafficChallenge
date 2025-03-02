using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject stopPanel;
    public void RestartGame()
    {
        AudioManager.Instance.PlaySFX("Button");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // or (0)
        AudioManager.Instance.MusicSource.mute = false;
    }
    public void Pause()
    {
        AudioManager.Instance.MusicSource.mute = true;
        AudioManager.Instance.PlaySFX("Button");
        Time.timeScale = 0;
        stopPanel.SetActive(true);
    }
    public void Resume()
    {
        AudioManager.Instance.MusicSource.mute = false;
        AudioManager.Instance.PlaySFX("Button");
        Time.timeScale = 1;
        stopPanel.SetActive(false);
    }
}
