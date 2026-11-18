using UnityEngine;

public class PlanetDataResetter : MonoBehaviour
{
    [SerializeField] private PlanetDataContainer m_AllPlanets;

    private void Awake()
    {
        m_AllPlanets.ResetAllPlanetData();
    }
}
