using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EndingType
{
    None,
    HomePlanetNeglected,
    Diplomacy,
    PopulationAbundance,
    ResourceAbundance,
    FoodAbundance,
    Balanced,
    EVIL,
}

public class GalaxyManager : MonoBehaviour
{
    [SerializeField] private PlanetData m_HomeData;
    [SerializeField] private PlanetDataContainer m_AllPlanetData;

    private void Start()
    {
        EvaluateAndTriggerEnding();
    }

    private void EvaluateAndTriggerEnding()
    {
        EndingType ending = DetermineEnding();
        TriggerEnding(ending);
    }

    private EndingType DetermineEnding()
    {
        List<PlanetData> planets = m_AllPlanetData.AllPlanets;

        int count = planets.Count;
        if (count == 0)
        {
            return EndingType.None;
        }

        float homePlanetSum = (m_HomeData.Food + m_HomeData.People + m_HomeData.Resource + m_HomeData.Diplomacy) / 4f;
        if (homePlanetSum < 20f)
        {
            return EndingType.HomePlanetNeglected;
        }

        float diplomacySum = 0;
        float populationSum = 0;
        float resourceSum = 0;
        float foodSum = 0;

        for (int i = 0; i < planets.Count; i++)
        {
            PlanetData planetData = planets[i];

            diplomacySum += planetData.Diplomacy;
            populationSum += planetData.People;
            resourceSum += planetData.Resource;
            foodSum += planetData.Food;
        }

        float averageDiplomacy = diplomacySum / count;
        float averagePopulation = populationSum / count;
        float averageResource = resourceSum / count;
        float averageFood = foodSum / count;

        if(homePlanetSum > 150)
        {
            return EndingType.EVIL;
        }

        if (averageDiplomacy >= 100f && averagePopulation >= 55f && averageResource >= 55f && averageFood >= 55f)
        {
            return EndingType.Diplomacy;
        }

        if (averagePopulation >= 80f && averageResource >= 60f)
        {
            return EndingType.PopulationAbundance;
        }

        if (averageResource >= 90f)
        {
            return EndingType.ResourceAbundance;
        }

        if (averageFood >= 85f && averagePopulation >= 70f)
        {
            return EndingType.FoodAbundance;
        }

        if (averageDiplomacy >= 70f && averagePopulation >= 70f && averageResource >= 70f && averageFood >= 70f)
        {
            return EndingType.Balanced;
        }

        return EndingType.None;
    }

    private void TriggerEnding(EndingType ending)
    {
        string sceneToLoad = "";

        switch (ending)
        {
            case EndingType.None:
                return;

            case EndingType.HomePlanetNeglected:
                sceneToLoad = SceneNames.ENDING_HOME_PLANET_NEGLECTED;
                break;

            case EndingType.Diplomacy:
                sceneToLoad = SceneNames.ENDING_DIPLOMACY;
                break;

            case EndingType.PopulationAbundance:
                sceneToLoad = SceneNames.ENDING_POPULATION_ABUNDANCE;
                break;

            case EndingType.ResourceAbundance:
                sceneToLoad = SceneNames.ENDING_RESOURCE_ABUNDANCE;
                break;

            case EndingType.FoodAbundance:
                sceneToLoad = SceneNames.ENDING_FOOD_ABUNDANCE;
                break;

            case EndingType.Balanced:
                sceneToLoad = SceneNames.ENDING_BALANCED;
                break;

            case EndingType.EVIL:
                sceneToLoad = SceneNames.ENDING_EVIL;
                break;

            default:
                break;
        }

        SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);
    }
}
