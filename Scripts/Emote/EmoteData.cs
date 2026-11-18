using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    Diplomacy,
    People,
    Resources,
    Food,
    Multiplier
}

[System.Serializable]
public class EmoteStat
{
    public StatType Type;
    public float Amount;
    [Range(0.5f, 2f)] public float Multiplier = 1f; // Only once per stat type.
}

[CreateAssetMenu(fileName = "EmoteData", menuName = "Rumor Diplomacy/EmoteData")]
public class EmoteData : ScriptableObject
{
    public string EmoteName;
    public Sprite Icon;
    public AudioClip Sound;
    public List<EmoteStat> StatChanges = new List<EmoteStat>();
    public KeyCode Hotkey;
}