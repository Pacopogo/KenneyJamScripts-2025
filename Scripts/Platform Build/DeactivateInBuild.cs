using UnityEngine;

public class DeactivateInBuild : MonoBehaviour
{
    private void Awake()
    {
        if (Application.isEditor == false) // Build version is active.
        {
            gameObject.SetActive(false);
        }
    }
}
