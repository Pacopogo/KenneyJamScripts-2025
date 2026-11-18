using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DiplomacyEvent : MonoBehaviour
{
    [SerializeField] private string m_PlayerTag = "Player";
    [SerializeField] private UnityEvent OnDiplomacy;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(m_PlayerTag))
        {
            return;
        }

        if (IsSceneLoaded(SceneNames.EMOTE_SEQUENCE))
        {
            return;
        }

        OnDiplomacy?.Invoke();
        SceneManager.LoadSceneAsync(SceneNames.EMOTE_SEQUENCE, LoadSceneMode.Additive);
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
