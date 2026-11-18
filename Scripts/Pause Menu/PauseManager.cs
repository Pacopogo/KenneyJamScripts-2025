using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private InputActionReference m_PauseAction;
    public static PauseManager Instance { get; private set; }

    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        m_PauseAction.action.performed += OnPauseTriggered;
        m_PauseAction.action.Enable();
    }

    private void OnDisable()
    {
        m_PauseAction.action.performed -= OnPauseTriggered;
    }

    private void Initialize()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnPauseTriggered(InputAction.CallbackContext context)
    {
        if (IsSceneLoaded(SceneNames.MAIN_MENU))
        {
            return;
        }

        if (IsSceneLoaded(SceneNames.PAUSE_MENU))
        {
            ClosePauseMenu();
        }
        else
        {
            OpenPauseMenu();
        }
    }

    private void OpenPauseMenu()
    {
        if (IsSceneLoaded(SceneNames.PAUSE_MENU) == true) // Menu is already open.
        {
            return;
        }

        SceneManager.LoadSceneAsync(SceneNames.PAUSE_MENU, LoadSceneMode.Additive);
        //Time.timeScale = 0f; // Handled in the menu controller.
    }

    private void ClosePauseMenu()
    {
        if (IsSceneLoaded(SceneNames.PAUSE_MENU) == false) // menu is already closed.
        {
            return;
        }

        SceneManager.UnloadSceneAsync(SceneNames.PAUSE_MENU);
        //Time.timeScale = 1f; // Handled in the menu controller.
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
