using System;
using UnityEngine;

namespace Budtender.Containers
{
    public static class Containers
    {
        //Containers for different classes in Budtender.
    }
    [Serializable]
    public class NpcSpawnChance
    {
        public NpcTemplate npcTemplate;
        [Range(0,1)]
        public float spawnChance;
    }
    [Serializable]
    public class NpcFixedSpawn
    {
        public NpcTemplate npcTemplate;
        public int customerIndex;
    }
}