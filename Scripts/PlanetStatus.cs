using UnityEngine;

public enum PlanetStates
{
    None,
    Thriving,
    Happy,
    Worry,
    Terrified,
    Distrust,
}

public class PlanetStatus : MonoBehaviour
{
    public PlanetStates PlanetState;
    [SerializeField] private PlanetData m_CurrentData;
    [SerializeField] private DisplayNPCEmote[] m_NPCs;

    private void Start()
    {
        m_CurrentData = GalaxyStateManager.Instance.GetCurrentData();
        SetPlanetStatus();
    }

    private void SetPlanetStatus()
    {
        if (m_CurrentData.Diplomacy <= 10)
        {
            PlanetState = PlanetStates.Distrust;

            foreach (DisplayNPCEmote npc in m_NPCs)
            {
                npc.SetSpriteToStatus(PlanetState);
            }
            return;
        }

        float totalSum = 
            m_CurrentData.Diplomacy +
            m_CurrentData.People +
            m_CurrentData.Resource +
            m_CurrentData.Food;

        totalSum = totalSum / 4;
        //Debug.Log(totalSum);

        if (totalSum >= 100)
        {
            PlanetState = PlanetStates.Thriving;
        }
        else if (totalSum >= 50)
        {
            PlanetState = PlanetStates.Happy;
        }
        else if(totalSum < 50 && totalSum > 10)
        {
            PlanetState = PlanetStates.Worry;
        }
        else if(totalSum <= 10)
        {
            PlanetState = PlanetStates.Terrified;
        }

        foreach (DisplayNPCEmote npc in m_NPCs)
        {
            npc.SetSpriteToStatus(PlanetState);
        }
    }
}
