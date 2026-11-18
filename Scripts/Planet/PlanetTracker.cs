using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlanetTracker : MonoBehaviour
{
    [SerializeField] private LayerMask m_layerMask;
    [SerializeField] private PlanetInfoDisplay m_PlanetInfoDisplay;
    [SerializeField] private PlanetData m_PlanetData;
    private bool hasChecked = false;

    [SerializeField] private TMP_Text m_PlanetNameText;

    [SerializeField] private UnityEvent OnTrack;
    [SerializeField] private UnityEvent OnTrackDisable;

    void FixedUpdate()
    {
        CheckForPlanet();
    }

    private void CheckForPlanet()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, m_layerMask))
        {

            Planet planet = hit.collider.gameObject.GetComponent<Planet>();
            m_PlanetData = planet.PlanetData;

            SetPlanetName();

            if (m_PlanetData.HasVisited && planet.PlanetData.Diplomacy >= 0)
            {
                Display();
            }

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);

            if (hasChecked)
                return;

            hasChecked = true;
            OnTrack?.Invoke();
        }
        else
        {

            HideDisplays();

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red);

            if (!hasChecked)
                return;

            hasChecked = false;
            OnTrackDisable?.Invoke();
        }
    }

    private void SetPlanetName()
    {
        m_PlanetNameText.gameObject.SetActive(true);
        if (m_PlanetData.HasVisited)
        {
            m_PlanetNameText.text = m_PlanetData.PlanetName;
        }
        else
        {
            m_PlanetNameText.text = "UNKNOWN";
        }
    }

    private void Display()
    {
        m_PlanetInfoDisplay.SetPlanetData(m_PlanetData);
        m_PlanetInfoDisplay.EnableDisplay();
    }

    private void HideDisplays()
    {
        m_PlanetInfoDisplay.DisableDisplay();
        m_PlanetData = null;
        m_PlanetNameText.gameObject.SetActive(false);
    }

}
