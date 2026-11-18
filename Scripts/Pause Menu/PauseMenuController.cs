using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject m_ConfirmationPanel;
    [SerializeField] private GameObject m_ResumeSelectable;
    [SerializeField] private GameObject m_CancelSelectable;

    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    public void OnResume()
    {
        SceneManager.UnloadSceneAsync(SceneNames.PAUSE_MENU);
    }

    public void OnReturnToMainMenu()
    {
        m_ConfirmationPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(m_CancelSelectable);
    }

    public void OnCancelReturnToMainMenu()
    {
        m_ConfirmationPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(m_ResumeSelectable);
    }

    public void OnConfirmReturnToMainMenu()
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
