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

        public List<Effects> unwatedEffects = new List<Effects>();
        public List<Flavors> unwantedFlavors = new List<Flavors>();

        public Vector2Int thcRange;
        public Vector2Int cbdRange;

        public Order(TasteProfile tasteProfile)
        {
            Category = tasteProfile.categoryPreference.GetWeightedRandomKey();
            /*
            desiredEffects = new List<Effects>(tasteProfile.preferredEffects);
            unwantedFlavors = new List<Flavors>(tasteProfile.dislikedFlavors);
            unwatedEffects = new List<Effects>(tasteProfile.dislikedEffects);
            desiredFlavors = new List<Flavors>(tasteProfile.preferredFlavors);
            thcRange = tasteProfile.thcRange;
            cbdRange = tasteProfile.cbdRange;
            */
        }
    }
}