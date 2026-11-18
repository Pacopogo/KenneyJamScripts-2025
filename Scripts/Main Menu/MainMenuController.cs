using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    public void OnPlay()
    {
        SceneManager.UnloadSceneAsync(SceneNames.MAIN_MENU);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
