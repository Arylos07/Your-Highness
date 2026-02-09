using Budtender.Orders;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;
using Budtender.Traits;
using System.Linq;

namespace Budtender.Shop
{
    public class NpcSpawner : MonoBehaviour
    {
        //this is kind of boilerplate, and may not be what the final NPC spawner looks like.

        //I think the main idea here will be to have a spawner template that this spawner uses.
        // this way, we can configure different spawning conditions later, but use the same manager.

        public NpcSpawnerTemplate npcSpawnerTemplate; //debug for now;
        public TasteProfile defaultTasteProfile; //debug for now;
        public Order order;

        [Button]
        void DebugOrder()
        {
            order = new Order(defaultTasteProfile);
        }

        //this function is the same as DebugOrder, but it uses inventory summary.
        [Button]
        void DebugWeightedOrder()
        {
            order = new Order(defaultTasteProfile, OrderManager.Instance.inventorySummary);
        }
    }
}