using Budtender.Traits;
using System;
using System.Collections.Generic;
using System.Text;
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

        /// <summary>
        /// Returns a clean, human-readable text representation of the inventory summary suitable for console/debug output.
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine("=== Inventory Summary ===");
            sb.AppendLine($"Product Count: {productCount}");

            sb.AppendLine($"THC Range: {FormatRange(thcRange)}");
            sb.AppendLine($"CBD Range: {FormatRange(cbdRange)}");

            sb.AppendLine(FormatList("Categories", allCategories));
            sb.AppendLine(FormatList("Effects", allEffects));
            sb.AppendLine(FormatList("Flavors", allFlavors));

            return sb.ToString();
        }

        private static string FormatRange(Vector2 range)
        {
            // Use two decimal places for clarity
            return $"{range.x:F2} - {range.y:F2}";
        }

        private static string FormatList<T>(string label, List<T> items)
        {
            if (items == null || items.Count == 0)
            {
                return $"{label}: None";
            }

            // Join enum or object names with comma and space
            return $"{label} ({items.Count}): {string.Join(", ", items)}";
        }
    }
}