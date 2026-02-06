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

        /*
         * After testing this, it does accurately represent the random chances in batches of 100. 
         * So I think this is the algorithm to go. Checking the profiler, I don't see many issues 
         * in terms of expense, so I think we can work with this. A part of me still wants to use
         * lists instead, but I think the unique key aspect of the dictionary is important, 
         * and I don't want to have to worry about validating that myself. God forbid I make an NPC
         * that has multiple of the same preferences, or players can breed a flower with 2 Happy traits.
         */

        [Button]
        void Brute()
        {
            //we're going to brute force RNG rolls here to find patterns.
            Dictionary<Category, int> categoryTest = new Dictionary<Category, int>(defaultTasteProfile.categoryPreference);
            List<Category> categories = new List<Category>();
            int testTarget = 100;
            //get 100 categories.
            for (int i = 0; i < testTarget; i++)
            {
                categories.Add(categoryTest.GetWeightedRandomKey());
            }

            foreach(Category cat in categories)
            {
                int count = categories.FindAll(x => x == cat).Count;
                Debug.Log($"{cat.ToString()}: {count}");
            }
        }
    }
}