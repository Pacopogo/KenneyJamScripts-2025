using UnityEngine;

public class SingleEventSystem : MonoBehaviour
{
    public static SingleEventSystem Instance { get; private set; }

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
