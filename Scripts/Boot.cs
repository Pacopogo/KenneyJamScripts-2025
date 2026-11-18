using UnityEngine;
using UnityEngine.SceneManagement;

public class Boot : MonoBehaviour
{
    private void Start()
    {
        LoadGame();
    }

    public void LoadGame()
    {
        Time.timeScale = 1f;

        if (IsSceneLoaded(SceneNames.PAUSE_MENU))
        {
            SceneManager.UnloadSceneAsync(SceneNames.PAUSE_MENU); // Just incase the pause menu is still open. Somehow???
        }

        SceneManager.LoadScene(SceneNames.PLANET_HOME, LoadSceneMode.Single); // Base game scene.

        SceneManager.LoadSceneAsync(SceneNames.MAIN_MENU, LoadSceneMode.Additive);
    }

    private bool IsSceneLoaded(string name)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == name)
            {
                return true;
            }
        }

        return false;
    }
}
