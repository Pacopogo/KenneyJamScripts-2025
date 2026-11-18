using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShipMovement : MonoBehaviour
{
    [SerializeField] private float m_Speed = 6;
    [SerializeField] private float m_BoostSpeed = 12;
    [SerializeField] private float m_RotationSpeed = 6;
    [SerializeField] private float m_RotationMultiplier = 10;

    //Y = Forward, X = Sideways
    [SerializeField] private Vector2 m_Dir;
    private bool m_IsBoost = false;

    [SerializeField] private Animator m_Animator;

    public void Move(InputAction.CallbackContext context)
    {
        m_Dir = context.ReadValue<Vector2>();
        m_Animator.SetFloat("Move", -m_Dir.x);
    }

    public void Boost(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            m_IsBoost = true;
        }
        else if(context.canceled)
        {
            m_IsBoost = false;
        }
    }

    private void FixedUpdate()
    {
        float speed = m_Dir.y * m_Speed * Time.fixedDeltaTime;
        float boostSpeed = m_Dir.y * m_BoostSpeed * Time.fixedDeltaTime;

        float rotSpeed = m_Dir.x * m_RotationSpeed * m_RotationMultiplier * Time.fixedDeltaTime;

        float moveSpeed = m_IsBoost ? boostSpeed : speed;

        transform.Translate(Vector3.forward * moveSpeed, Space.Self);
        transform.Rotate(Vector2.up * rotSpeed, Space.Self);
    }
}
