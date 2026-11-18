using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "All Planets", menuName = "PlanetData/Container", order = 1)]
public class PlanetDataContainer : ScriptableObject
{
    [SerializeField] private PlanetData m_HomePlanetData;
    public PlanetData HomePlanetData => m_HomePlanetData;
    [SerializeField] private List<PlanetData> m_AllPlanets = new List<PlanetData>();
    public List<PlanetData> AllPlanets => m_AllPlanets;

    public void ResetAllPlanetData()
    {
        for (int i = 0; i < m_AllPlanets.Count; i++)
        {
            PlanetData data = m_AllPlanets[i];

            if (data == m_HomePlanetData)
            {
                continue;
            }

            data.HasVisited = false;
            data.Diplomacy = Random.Range(50, 80);
            data.People = Random.Range(20, 45);
            data.Resource = Random.Range(20, 45);
            data.Food = Random.Range(20, 45);
        }

        ResetHomePlanet();
    }

    private void ResetHomePlanet()
    {
        m_HomePlanetData.Food = 30;
        m_HomePlanetData.People = 30;
        m_HomePlanetData.Resource = 30;
        m_HomePlanetData.Diplomacy = 100;
    }
}
