using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class SimpleAudioPlayer : MonoBehaviour
{
    private AudioSource m_AudioSource;
    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void PlayClip(AudioClip clip)
    {
        m_AudioSource.clip = clip;
        m_AudioSource.Play();
    }
}
