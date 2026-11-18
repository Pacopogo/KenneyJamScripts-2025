using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHoverColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private List<Graphic> m_Targets = new List<Graphic>();
    [SerializeField] private Color m_HighlightColor = Color.yellow;
    [SerializeField] private Color m_DefaultColor = Color.white;

    private int m_HoverSources = 0; // Tracks multiple input sources (pointer + selection)

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_HoverSources++;
        UpdateColor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_HoverSources = Mathf.Max(0, m_HoverSources - 1);
        UpdateColor();
    }

    public void OnSelect(BaseEventData eventData)
    {
        m_HoverSources++;
        UpdateColor();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        m_HoverSources = Mathf.Max(0, m_HoverSources - 1);
        UpdateColor();
    }

    private void UpdateColor()
    {
        Color color = m_HoverSources > 0 ? m_HighlightColor : m_DefaultColor;

        for (int i = 0; i < m_Targets.Count; i++)
        {
            if (m_Targets[i] != null)
            {
                m_Targets[i].color = color;
            }
        }
    }
}
