using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "NpcTemplate", menuName = "Npc Template")]
public class NpcTemplate : ScriptableObject
{
    //This is such a boilerplate class for expansion later.

    public TasteProfile tasteProfile;
    public GameObject model;
    public Faction faction;

    [InfoBox("In the future, this may become a percentage, so the NPC spawns with a percentage of what the player owns or something. This way, the play isn't dealing with lowballers in the mid/late game.")]
    [MinMaxSlider(0, 100, true)]
    public Vector2 moneyRange = new Vector2(10, 100);
}
