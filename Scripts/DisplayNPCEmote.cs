using UnityEngine;

public class DisplayNPCEmote : MonoBehaviour
{
    [SerializeField] private PlanetStates m_PlanetStates;
    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private NPCEmoteList m_Sprites;
    [SerializeField] private Animator m_Animator;

    public void SetSpriteToStatus(PlanetStates status)
    {
        m_PlanetStates = status;
        m_SpriteRenderer.enabled = true;

        int rnd;
        switch (m_PlanetStates)
        {
            case PlanetStates.None:
                m_SpriteRenderer.enabled = false;
                break;
            case PlanetStates.Thriving:
                rnd = Random.Range(0, m_Sprites.Thriving.Length);
                m_SpriteRenderer.sprite = m_Sprites.Thriving[rnd];

                break;
            case PlanetStates.Happy:
                rnd = Random.Range(0, m_Sprites.Happy.Length);
                m_SpriteRenderer.sprite = m_Sprites.Happy[rnd];

                break;
            case PlanetStates.Worry:
                rnd = Random.Range(0, m_Sprites.Worry.Length);
                m_SpriteRenderer.sprite = m_Sprites.Worry[rnd];

                break;
            case PlanetStates.Terrified:
                rnd = Random.Range(0, m_Sprites.Terrified.Length);
                m_SpriteRenderer.sprite = m_Sprites.Terrified[rnd];

                break;
            case PlanetStates.Distrust:
                m_SpriteRenderer.enabled = false;

                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.GetComponent<PlayerMovementGround>())
            return;

        m_Animator.SetBool("Close", true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.GetComponent<PlayerMovementGround>())
            return;

        m_Animator.SetBool("Close", false);
    }
}
