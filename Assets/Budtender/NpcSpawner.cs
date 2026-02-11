using Budtender.Orders;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;
using Budtender.Traits;
using System.Linq;
using System;

namespace Budtender.Shop
{
    public class NpcSpawner : MonoBehaviour
    {
        //this is kind of boilerplate, and may not be what the final NPC spawner looks like.

        //I think the main idea here will be to have a spawner template that this spawner uses.
        // this way, we can configure different spawning conditions later, but use the same manager.

        public NpcSpawnerTemplate template; //debug for now;
        public TasteProfile defaultTasteProfile; //debug for now;
        public Order order;

        [SerializeField]
        public List<Customer> Customers = new List<Customer>();

        private void Start()
        {
            GameManager.Instance.OnDayAdvanced += GenerateCustomers;
        }

        [Button]
        void GenerateCustomers(DateTime date)
        {
            //we'll need to do some logic to remove customers before starting a new list.
            InventorySummary summary = OrderManager.Instance.SummarizeInventory();
            if(summary == null)
            {
                Debug.LogError("Inventory summary is null. Cannot generate customers.");
                return;
            }
            Customers = template.GenerateCustomers(summary); //for now
        }

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