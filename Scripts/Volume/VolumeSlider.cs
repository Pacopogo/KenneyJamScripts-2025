using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private Slider m_Slider;
    [SerializeField] private AudioMixerGroup m_AudioMixerGroup;

    //Instead of setting the slider min / max values to -80 and 20, set them to min 0.0001 and max 1.
    private const float m_SliderMinimum = 0.0001f;
    private const float m_SliderMaximum = 1f;

    public const string VOLUME_PROPERTY = "Volume"; //Exposed parameter name is mixer group name + this variable (Ex: (Mixer Group) Music + VolumeProperty;
    public const float DEFAULT_VOLUME = 0.5f;

    private void OnEnable()
    {
        m_Slider.minValue = m_SliderMinimum;
        m_Slider.maxValue = m_SliderMaximum;

        string parameterName = GetExposedMixerGroupVolumeParameterName(m_AudioMixerGroup.name);
        float mixerVolume = PlayerPrefs.GetFloat(parameterName, DEFAULT_VOLUME);

        m_Slider.value = mixerVolume;
        m_Slider.onValueChanged.AddListener(delegate { ChangeVolume(); });
    }

    private void Start()
    {
        ChangeVolume();
    }

    private void OnDisable()
    {
        m_Slider.onValueChanged.RemoveListener(delegate { ChangeVolume(); });
    }

    private void ChangeVolume()
    {
        SetAudioMixerVolume(m_AudioMixerGroup, m_Slider.value);
    }

    public void SetAudioMixerVolume(AudioMixerGroup group, float volume)
    {
        //Instead of setting the slider min / max values to -80 and 20, set them to min 0.0001 and max 1.
        float valueToSet = Mathf.Log10(volume) * 20f;

        string parameterName = GetExposedMixerGroupVolumeParameterName(group.name);
        group.audioMixer.SetFloat(parameterName, valueToSet);

        PlayerPrefs.SetFloat(parameterName, volume);
        //Debug.Log("Set " + group.name + " volume to: " + volume);
    }

    public static string GetExposedMixerGroupVolumeParameterName(string groupName)
    {
        return groupName + VOLUME_PROPERTY;
    }
}
