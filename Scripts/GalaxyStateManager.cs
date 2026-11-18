using System.Collections.Generic;
using UnityEngine;

public class GalaxyStateManager : MonoBehaviour
{

    [SerializeField] private PlanetData m_HomeData;
    [SerializeField] private PlanetData m_CurrentData;
    [SerializeField] private PlanetData[] m_Data;

    public static GalaxyStateManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        m_CurrentData = m_HomeData;
    }

    private void OnEnable()
    {
        EmoteEvents.CompletedSequence += UpdateCurrentPlanetData;
    }

    private void OnDisable()
    {
        EmoteEvents.CompletedSequence -= UpdateCurrentPlanetData;
    }

    [ContextMenu("Reset Planets")]
    public void ResetPlanets()
    {
        foreach(PlanetData planet in m_Data)
        {
            planet.HasVisited = false;
        }
    }

    public void SetCurrentData(PlanetData data) => m_CurrentData = data;
    public PlanetData GetCurrentData() => m_CurrentData;

    private void UpdateCurrentPlanetData(Dictionary<StatType, float> addedStats)
    {
        if (m_CurrentData == null)
        {
            return;
        }

        Debug.Log($"Updated {m_CurrentData.PlanetName} data.");

        m_CurrentData.Diplomacy += addedStats[StatType.Diplomacy];
        m_CurrentData.People    += addedStats[StatType.People];
        m_CurrentData.Resource  += addedStats[StatType.Resources];
        m_CurrentData.Food      += addedStats[StatType.Food];

        m_HomeData.Diplomacy    -= addedStats[StatType.Diplomacy];
        m_HomeData.People       -= addedStats[StatType.People];
        m_HomeData.Resource     -= addedStats[StatType.Resources];
        m_HomeData.Food         -= addedStats[StatType.Food];
    }
}
