using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Faction", menuName = "Factions/New Faction")]
public class Faction : ScriptableObject
{
    [InfoBox("This object isn't really used right now, but in the future it will be The idea is that you can have Novirans, Keldarians, Jubelans, etc.  even Architects as customers; each with their own quests and preferences. We may want to make a FactionManager later to handle faction relations as well as a global taste profile for anyone in the faction. So Architects may prefer high THC content, while Novirans prioritize relaxation.\n\nAlthough, I think much of that will be dictated by the NPC templates themselves, and the faction will just be a way to group them together for spawn purposes.", InfoMessageType.Warning)]

    [Tooltip("If faction is not enabled then this faction does not participate in spawn.")]
    [SerializeField]
    private bool enabled = true;

    [Tooltip("Faction spawn chance dependently from karma. Spawn chances of all factions will be normalized at spawn moment.\nx-axis - karma, from -100 to 100.y-axis - spawn chance (from 0 to inf).")]
    public AnimationCurve spawnChance;

    [Header("One shot NPCs:")]
    [Range(0f, 100f)]
    public float spawnChanceOneShotNpc = 10f;

    [Tooltip("If this flag is false then one shot NPCs will be spawned in order, not randomly.")]
    public bool randomSpawnOneShotNpc = true;

    [Tooltip("List with one shot NPC's lists for each chapter (first element at this list corresponds to first chapter).\nNPCs from current or previous chapters can be spawned only.")]
    public List<NpcTemplateList> oneShotNpc = new List<NpcTemplateList>();

    [NonSerialized]
    private float maxSpawnChance = float.MinValue;

    [NonSerialized]
    private float minSpawnChance = float.MaxValue;


    /*
     * this object isn't really used right now, but in the future it will be
     * The idea is that you can have Novirans, Keldarians, Jubelans, etc. 
     * even Architects as customers; each with their own quests and preferences.
     * We may want to make a FactionManager later to handle faction relations
     * as well as a global taste profile for anyone in the faction.
     * So Architects may prefer high THC content, while Novirans prioritize relaxation.
     * 
     * Although, I think much of that will be dictated by the NPC templates themselves,
     * and the faction will just be a way to group them together for spawn purposes.
     */
}
