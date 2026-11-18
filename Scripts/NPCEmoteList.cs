using UnityEngine;

[CreateAssetMenu(fileName = "NPC", menuName = "NPCEmote/NPCEmote", order = 1)]
public class NPCEmoteList : ScriptableObject
{
    public Sprite[] Thriving;
    public Sprite[] Happy;
    public Sprite[] Worry;
    public Sprite[] Terrified;
}
