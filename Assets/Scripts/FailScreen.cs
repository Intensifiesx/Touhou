using UnityEngine;
using UnityEngine.SceneManagement;
public class FailScreen : MonoBehaviour
{
    public void Setup()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
