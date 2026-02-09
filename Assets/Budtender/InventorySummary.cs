using Budtender.Traits;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Budtender.Orders
{
    /// <summary>
    /// This class is a summary of the player's inventory, and what orders they can complete.
    /// Over time, we will want to track crafted items in here.
    /// </summary>
    [Serializable]
    public class InventorySummary
    {
        //total number of sellable objects
        public List<Category> allCategories = new List<Category>();
        public int productCount;
        public List<Effects> allEffects = new List<Effects>();
        public List<Flavors> allFlavors = new List<Flavors>();
        public Vector2 thcRange;
        public Vector2 cbdRange;
        //we will eventually want to put crafted items and product types here.
    }
}