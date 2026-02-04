using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Faction", menuName = "Factions/New Faction")]
public class Faction : ScriptableObject
{
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
}
