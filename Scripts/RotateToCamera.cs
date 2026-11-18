using UnityEngine;

public class RotateToCamera : MonoBehaviour
{
    [SerializeField] private GameObject cameraObj;
    void Update()
    {
        transform.LookAt(cameraObj.transform.position, Vector3.left);
    }
}
