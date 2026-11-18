using UnityEngine;
using TMPro;

public class PlanetInfoDisplay : MonoBehaviour
{
    [SerializeField] private GameObject[] textObjs;

    [SerializeField] private TMP_Text m_DiplomacyText;
    [SerializeField] private TMP_Text m_PeopleText;
    [SerializeField] private TMP_Text m_ResourceText;
    [SerializeField] private TMP_Text m_FoodText;

    private PlanetData m_PlanetData;

    public void EnableDisplay()
    {
        foreach (var obj in textObjs)
        {
            obj.SetActive(true);
        }

        SetDisplayData();
    }
    public void DisableDisplay()
    {
        foreach (var obj in textObjs)
        {
            obj.SetActive(false);
        }
    }
    public void SetPlanetData(PlanetData data) => m_PlanetData = data;
    private void SetDisplayData()
    {
        if(m_PlanetData == null)
        {
            DisableDisplay();
            return;
        }

        m_DiplomacyText.text    = m_PlanetData.Diplomacy.ToString();
        m_PeopleText.text       = m_PlanetData.People.ToString();
        m_ResourceText.text     = m_PlanetData.Resource.ToString();
        m_FoodText.text         = m_PlanetData.Food.ToString();
    }
}
