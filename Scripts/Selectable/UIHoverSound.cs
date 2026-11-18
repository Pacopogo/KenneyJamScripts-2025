using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIHoverSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [Header("Audio")]
    [SerializeField] private AudioMixerGroup m_AudioMixerGroup;
    public AudioMixerGroup AudioMixerGroup => m_AudioMixerGroup;
    [SerializeField] private AudioClip m_HighlightSound;
    public AudioClip HighlightSound => m_HighlightSound;
    [SerializeField] private AudioClip m_DeselectSound;
    public AudioClip DeselectSound => m_DeselectSound;
    private AudioSource m_AudioSource;

    private int m_HighlightSources = 0; // Tracks multiple input sources (pointer + selection)

    private void Awake()
    {
        m_AudioSource = gameObject.AddComponent<AudioSource>();
        m_AudioSource.loop = false;
        m_AudioSource.playOnAwake = false;
        m_AudioSource.outputAudioMixerGroup = m_AudioMixerGroup;
    }

    public void Initialize(AudioMixerGroup audioMixerGroup, AudioClip highlightSound, AudioClip deselectSound)
    {
        m_AudioMixerGroup = audioMixerGroup;
        m_HighlightSound = highlightSound;
        m_DeselectSound = deselectSound;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_HighlightSources++;
        PlayClip(m_HighlightSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_HighlightSources = Mathf.Max(0, m_HighlightSources - 1);
        if (m_HighlightSources == 0)
        {
            PlayClip(m_DeselectSound);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        m_HighlightSources++;
        PlayClip(m_HighlightSound);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        m_HighlightSources = Mathf.Max(0, m_HighlightSources - 1);
        if (m_HighlightSources == 0)
        {
            PlayClip(m_DeselectSound);
        }
    }

    private void PlayClip(AudioClip clip)
    {
        if (m_AudioSource != null && clip != null)
        {
            m_AudioSource.PlayOneShot(clip);
        }
    }
}
