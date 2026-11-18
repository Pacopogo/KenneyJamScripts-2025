using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPlanet : MonoBehaviour
{
    [SerializeField] private string m_Scene = "Planet(HOME)";
    [SerializeField] private string m_PlayerTag = "Player";
    private Planet m_Planet;
    private PlanetData m_PlanetData;
    private void Start()
    {
        m_Planet = GetComponent<Planet>();

        if (!m_Planet)
            return;

        m_PlanetData = m_Planet.PlanetData;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(m_PlayerTag))
            return;

        m_Planet = GetComponent<Planet>();

        if (m_Planet)
        {
            m_PlanetData = m_Planet.PlanetData;
            GalaxyStateManager.Instance.SetCurrentData(m_PlanetData);
        }

        if (m_PlanetData)
        {
            //Set the planet up
            m_PlanetData.HasVisited = true;
        }

        SceneManager.LoadSceneAsync(m_Scene);
    }
}
