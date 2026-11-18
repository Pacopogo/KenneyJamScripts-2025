using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementGround : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private SpriteRenderer m_Sprite;

    [SerializeField] private float m_MoveSpeed = 2;
    [SerializeField] private Vector2 m_Dir;
    public void Move(InputAction.CallbackContext context)
    {
        m_Dir = context.ReadValue<Vector2>();
        m_Animator.SetFloat("Move", m_Dir.magnitude);

        if (m_Dir.x > 0)
        {
            m_Sprite.flipX = true;
        }
        else if( m_Dir.x < 0)
        {
            m_Sprite.flipX = false;
        }
    }

    private void FixedUpdate()
    {
        MoveOnDirection();
    }

    private void MoveOnDirection()
    {
        float speedX = m_Dir.x * m_MoveSpeed * Time.fixedDeltaTime;
        float speedY = m_Dir.y * m_MoveSpeed * Time.fixedDeltaTime;

        transform.Translate(Vector3.forward * speedX, Space.Self);
        transform.Translate(Vector3.left * speedY, Space.Self);
    }
}
