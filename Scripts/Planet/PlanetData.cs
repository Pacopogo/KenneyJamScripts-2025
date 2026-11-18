using UnityEngine;

[CreateAssetMenu(fileName = "Planet", menuName = "PlanetData/PlanetData", order = 1)]
public class PlanetData : ScriptableObject 
{
    [Header("Planet info")]
    public string PlanetName;
    public bool HasVisited = false;

    [Header("Planet Values")]
    public float Diplomacy;
    public float People;
    public float Resource;
    public float Food;
}
