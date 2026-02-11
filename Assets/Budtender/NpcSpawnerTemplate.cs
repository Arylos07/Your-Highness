using Budtender.Containers;
using Budtender.Orders;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
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

        public int GetCustomerCount()
        {
            return Random.Range(baseCustomersPerDay.x, baseCustomersPerDay.y + 1);
        }

        public List<Customer> GenerateCustomers(InventorySummary summary)
        {
            int customerCount = GetCustomerCount();

            // initialize list with null placeholders (index corresponds to spawn order)
            List<Customer> customers = new List<Customer>(Enumerable.Repeat<Customer>(null, customerCount));

            // --- Place fixed spawns (non -1 indexes first) ---
            if (fixedNpcs != null)
            {
                foreach (var fixedSpawn in fixedNpcs)
                {
                    if (fixedSpawn == null || fixedSpawn.npcTemplate == null)
                    {
                        continue;
                    }

                    // handle explicit positions (1-based in data)
                    if (fixedSpawn.customerIndex > 0)
                    {
                        int target = fixedSpawn.customerIndex - 1;

                        // clamp into bounds
                        if (target < 0) target = 0;
                        if (target >= customerCount) target = customerCount - 1;

                        if (customers[target] == null)
                        {
                            customers[target] = new Customer(fixedSpawn.npcTemplate, summary);
                        }
                        else
                        {
                            // find next available slot to the right, then to the left
                            int found = -1;
                            for (int i = target + 1; i < customerCount; i++)
                            {
                                if (customers[i] == null)
                                {
                                    found = i;
                                    break;
                                }
                            }
                            if (found == -1)
                            {
                                for (int i = target - 1; i >= 0; i--)
                                {
                                    if (customers[i] == null)
                                    {
                                        found = i;
                                        break;
                                    }
                                }
                            }

                            if (found != -1)
                            {
                                customers[found] = new Customer(fixedSpawn.npcTemplate, summary);
                            }
                            // if no free slot found, skip this fixed spawn (too many fixeds for day)
                        }
                    }
                }

                // --- Place fixed spawns marked -1 at end-of-day slots (from the end backwards) ---
                foreach (var fixedSpawn in fixedNpcs)
                {
                    if (fixedSpawn == null || fixedSpawn.npcTemplate == null)
                    {
                        continue;
                    }

                    if (fixedSpawn.customerIndex == -1)
                    {
                        // find last available slot (search from end)
                        int found = -1;
                        for (int i = customerCount - 1; i >= 0; i--)
                        {
                            if (customers[i] == null)
                            {
                                found = i;
                                break;
                            }
                        }

                        if (found != -1)
                        {
                            customers[found] = new Customer(fixedSpawn.npcTemplate, summary);
                        }
                        // otherwise skip if no free slot left
                    }
                }
            }

            // --- Fill remaining slots with random spawns using weighted selection ---
            // prepare clean list of candidates
            var candidates = (randomNpcs ?? new List<NpcSpawnChance>())
                .Where(x => x != null && x.npcTemplate != null)
                .ToList();

            if (candidates.Count > 0)
            {
                float totalWeight = candidates.Sum(x => x.spawnChance);

                bool uniformFallback = totalWeight <= 0f;

                for (int i = 0; i < customerCount; i++)
                {
                    if (customers[i] != null)
                        continue; // already filled by fixed spawn

                    NpcTemplate selectedTemplate = null;

                    if (uniformFallback)
                    {
                        int idx = Random.Range(0, candidates.Count);
                        selectedTemplate = candidates[idx].npcTemplate;
                    }
                    else
                    {
                        float pick = Random.Range(0f, totalWeight);
                        float acc = 0f;
                        foreach (var c in candidates)
                        {
                            acc += c.spawnChance;
                            if (pick <= acc)
                            {
                                selectedTemplate = c.npcTemplate;
                                break;
                            }
                        }

                        // safety fallback
                        if (selectedTemplate == null)
                        {
                            selectedTemplate = candidates[candidates.Count - 1].npcTemplate;
                        }
                    }

                    customers[i] = new Customer(selectedTemplate, summary);
                }
            }
            else
            {
                // No random NPCs available: leave any remaining slots null (they'll be removed below).
            }

            // Remove any null placeholders (in case there were fewer NPCs available than slots)
            customers.RemoveAll(c => c == null);

            return customers;
        }
    }
}