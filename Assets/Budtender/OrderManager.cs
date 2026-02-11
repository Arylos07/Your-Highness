using UnityEngine;
using UnityCommunity.UnitySingleton;
using System.Collections.Generic;
using System.Linq;
using Budtender.Traits;
using Sirenix.OdinInspector;

namespace Budtender.Orders
{
    public class OrderManager : MonoSingleton<OrderManager>
    {
        //we won't use this often, so let's just do this. We can always change it later if we need to.
        // Kind of hard to do a cache method here, but I'd like to investigate it.
        //debug, revert to inventorySummary => SummarizeInventory();
        public InventorySummary inventorySummary;

        //this function doesn't handle different products yet, but we'll get there
        //[Button]
        public InventorySummary SummarizeInventory()
        {
            if(GameManager.Instance.ProductInventory.Count == 0)
            {
                Debug.LogWarning("No products in inventory! No summary was generated!");
                //this is dangerous, returning null, but we need to tell whatever was wanting a summary that the player has no inventory.
                return null; 
            }

            InventorySummary summary = new InventorySummary();

            List<ProductSlot> flowers = new List<ProductSlot>(GameManager.Instance.ProductInventory);

            //this will be used so we don't spawn more customers than the player can handle. While they can turn away customers, I'd rather it not be common.
            summary.productCount = flowers.Count;

            summary.allCategories = flowers.Select(f => f.flower.Category).Distinct().ToList();

            //good old linq to get a distinct list of flavors and effects. We can check taste profiles to only include these. That should count dislikes too some degree.
            summary.allFlavors = flowers.SelectMany(f => f.flower.Flavors).Distinct().ToList();
            summary.allEffects = flowers.SelectMany(f => f.flower.Effects).Distinct().ToList();

             
            //get the overall cannabanoid range so we know the widest range the player can satisfy. I have not programmed hard preferences like I did flavors and effects, but it may possibly become a part of it.
            summary.thcRange = new Vector2(
                flowers.Min(f => f.flower.Thc),
                flowers.Max(f => f.flower.Thc)
            );
            summary.cbdRange = new Vector2(
                flowers.Min(f => f.flower.Cbd),
                flowers.Max(f => f.flower.Cbd)
            );

            inventorySummary = summary; //debug
            return summary;
        }
    }
}