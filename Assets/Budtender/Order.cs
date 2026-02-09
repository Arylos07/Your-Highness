using UnityEngine;
using Budtender.Traits;
using System.Collections.Generic;
using System;

namespace Budtender.Orders
{
    //this class is basically what the customer is looking for. Think of this as a simple flower and we compare if the selected product matches.
    //TODO: this needs to handle more product types than just flower. We'll get there when we build the enum for product types.
    [Serializable]
    public class Order
    {
        public Category Category;
        public List<Effects> desiredEffects = new List<Effects>();
        public List<Flavors> desiredFlavors = new List<Flavors>();

        public List<Effects> unwantedEffects = new List<Effects>();
        public List<Flavors> unwantedFlavors = new List<Flavors>();

        public Vector2 thcRange;
        public Vector2 cbdRange;

        //not sure if these will be needed, but I might as well
        public int thcMedian;
        public int cbdMedian;

        public Order(TasteProfile tasteProfile)
        {
            Category = tasteProfile.categoryPreference.GetWeightedRandomKey();
            desiredEffects = tasteProfile.effectsPreference.GetWeightedRandomKeys();
            desiredFlavors = tasteProfile.flavorsPreference.GetWeightedRandomKeys();
            unwantedEffects = tasteProfile.effectsDislikes.GetWeightedRandomKeys();
            unwantedFlavors = tasteProfile.flavorsDislikes.GetWeightedRandomKeys();
            thcRange = tasteProfile.thcPreference;
            cbdRange = tasteProfile.cbdPreference;

            thcMedian = tasteProfile.getThcMedian();
            cbdMedian = tasteProfile.getCbdMedian();
        }

        // Constructor now chains to the TasteProfile constructor so this instance is initialized first,
        // then refined to match the supplied inventorySummary.
        public Order(TasteProfile tasteProfile, InventorySummary inventorySummary) : this(tasteProfile)
        {
            //is this necessary?
            if (inventorySummary == null)
            {
                return;
            }

            // check category first. If they want a category we don't have, reassign to a random available category (if any).
            if (inventorySummary.allCategories != null && inventorySummary.allCategories.Count > 0)
            {
                if (!inventorySummary.allCategories.Contains(Category))
                {
                    Category = inventorySummary.allCategories[UnityEngine.Random.Range(0, inventorySummary.allCategories.Count)];
                }
            }

            // check effects next. Remove desired/unwanted effects that aren't present in inventory.
            if (inventorySummary.allEffects != null)
            {
                desiredEffects.RemoveAll(e => !inventorySummary.allEffects.Contains(e));
                unwantedEffects.RemoveAll(e => !inventorySummary.allEffects.Contains(e));
            }
            else
            {
                desiredEffects.Clear();
                unwantedEffects.Clear();
            }

            // check flavors next. Remove desired/unwanted flavors that aren't present in inventory.
            if (inventorySummary.allFlavors != null)
            {
                desiredFlavors.RemoveAll(f => !inventorySummary.allFlavors.Contains(f));
                unwantedFlavors.RemoveAll(f => !inventorySummary.allFlavors.Contains(f));
            }
            else
            {
                desiredFlavors.Clear();
                unwantedFlavors.Clear();
            }

            // clamp thc and cbd ranges to the intersection with inventory ranges.
            // Use Max for lower bound and Min for upper bound to get the overlap.
            thcRange.x = Mathf.Min(thcRange.x, inventorySummary.thcRange.x);
            thcRange.y = Mathf.Max(thcRange.y, inventorySummary.thcRange.y);
            cbdRange.x = Mathf.Min(cbdRange.x, inventorySummary.cbdRange.x);
            cbdRange.y = Mathf.Max(cbdRange.y, inventorySummary.cbdRange.y);

            // If there is no overlap (min > max) revert to inventory range so the order remains valid.
            if (thcRange.x > thcRange.y && inventorySummary.thcRange.x <= inventorySummary.thcRange.y)
            {
                thcRange = inventorySummary.thcRange;
            }

            if (cbdRange.x > cbdRange.y && inventorySummary.cbdRange.x <= inventorySummary.cbdRange.y)
            {
                cbdRange = inventorySummary.cbdRange;
            }

            // Recompute medians to reflect final ranges.
            //thcMedian = Mathf.RoundToInt((thcRange.x + thcRange.y) / 2f);
            //cbdMedian = Mathf.RoundToInt((cbdRange.x + cbdRange.y) / 2f);
        }
    }
}