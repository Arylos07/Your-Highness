using Budtender.Containers;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Budtender.Shop
{
    /// <summary>
    /// This template dictates the NpcSpawner behavior for a day. These settings are modified by reputation, events, etc after the fact. Different templates can be used to alter spawner behavior based on game flow and difficulty.
    /// </summary>
    [CreateAssetMenu(fileName = "NpcSpawnerTemplate", menuName = "Budtender/Npc Spawner Template")]
    public class NpcSpawnerTemplate : ScriptableObject
    {
        //4-20 closely matches Potion Craft, but we can adjust it later if needed.
        // This is a base chance, not accounting for reputation, events, etc. 
        [MinMaxSlider(4, 20, true)]
        public Vector2Int baseCustomersPerDay = new Vector2Int(4, 6);

        //dictionaries don't serialize here, so we're using this instead.

        /// <summary>
        /// List of NPCs to spawn randomly. float is the chance to spawn this npc, between 0 and 1.
        /// </summary>
        public List<NpcSpawnChance> randomNpcs; // A dictionary to hold NPC templates and their spawn chances
        /// <summary>
        /// List of NPCs to spawn at fixed intervals. int is at what customer number to spawn this npc.
        /// </summary>
        /// I think having int = -1 means to spawn this npc at the end of the day, after all customers have been served.
        public List<NpcFixedSpawn> fixedNpcs;

        /* add an input so that tastes can be overridden. 
         * This way, we can have days where customers will have a strong preference or dislike for a trait.
         * For example, a festival is coming up, so a lot of sativa requests with happy and giggly traits.
         */
    }
}