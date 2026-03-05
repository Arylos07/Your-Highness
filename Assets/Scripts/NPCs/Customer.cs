using Budtender;
using Budtender.Orders;
using Budtender.Traits;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public class Customer
{
    public Order order;
    public int money;
    public TasteProfile tasteProfile; //may not be needed, but let's include it

    public enum CustomerState { Waiting, Served, TurnedAway }
    public CustomerState state = CustomerState.Waiting;

    //ignores the current inventory summary
    public Customer(NpcTemplate template)
    {
        tasteProfile = template.tasteProfile;
        money = Random.Range(template.moneyRange.x, template.moneyRange.y + 1);
        // order is intentionally left null until GenerateOrder is called
    }

    /// <summary>
    /// Generates this customer's order against the current inventory summary.
    /// Call this when the customer steps up to be served, not at construction time.
    /// </summary>
    public void GenerateOrder(InventorySummary inventorySummary)
    {
        order = new Order(tasteProfile, inventorySummary);
    }

    /// <summary>
    /// Evaluates a product against this customer's order.
    /// Returns a SaleResult with accepted/rejected, payment, and deduction breakdown.
    /// Dislikes of 100 are hard rejects (customer will not buy).
    /// Other mismatches reduce payment proportionally.
    /// </summary>
    public SaleResult EvaluateProduct(Product product)
    {
        SaleResult result = new SaleResult();
        result.accepted = true;
        float paymentMultiplier = 1f;

        // --- Hard rejects: dislike value of 100 means the customer will never buy ---
        if (tasteProfile.categoryDislikes != null && tasteProfile.categoryDislikes.TryGetValue(product.Category, out int catDislike))
        {
            if (catDislike >= 100)
            {
                result.accepted = false;
                result.rejectReason = $"Customer absolutely refuses {product.Category.ToCamelCaseString()} products.";
                result.payment = 0;
                return result;
            }
        }

        if (tasteProfile.effectsDislikes != null && product.Effects != null)
        {
            foreach (var effect in product.Effects)
            {
                if (tasteProfile.effectsDislikes.TryGetValue(effect, out int effectDislike) && effectDislike >= 100)
                {
                    result.accepted = false;
                    result.rejectReason = $"Customer absolutely refuses products with the {effect.ToCamelCaseString()} effect.";
                    result.payment = 0;
                    return result;
                }
            }
        }

        if (tasteProfile.flavorsDislikes != null && product.Flavors != null)
        {
            foreach (var flavor in product.Flavors)
            {
                if (tasteProfile.flavorsDislikes.TryGetValue(flavor, out int flavorDislike) && flavorDislike >= 100)
                {
                    result.accepted = false;
                    result.rejectReason = $"Customer absolutely refuses products with the {flavor.ToCamelCaseString()} flavor.";
                    result.payment = 0;
                    return result;
                }
            }
        }

        // --- Category mismatch ---
        if (product.Category != order.Category)
        {
            paymentMultiplier -= 0.15f;
            result.deductions.Add($"Wrong category: wanted {order.Category.ToCamelCaseString()}, got {product.Category.ToCamelCaseString()} (-15%)");
        }

        // --- Desired effects: deduct for each missing desired effect ---
        if (order.desiredEffects != null && order.desiredEffects.Count > 0)
        {
            float perEffectWeight = 0.10f;
            foreach (var desired in order.desiredEffects)
            {
                if (product.Effects == null || !product.Effects.Contains(desired))
                {
                    paymentMultiplier -= perEffectWeight;
                    result.deductions.Add($"Missing desired effect: {desired.ToCamelCaseString()} (-{perEffectWeight * 100:F0}%)");
                }
            }
        }

        // --- Desired flavors: deduct for each missing desired flavor ---
        if (order.desiredFlavors != null && order.desiredFlavors.Count > 0)
        {
            float perFlavorWeight = 0.10f;
            foreach (var desired in order.desiredFlavors)
            {
                if (product.Flavors == null || !product.Flavors.Contains(desired))
                {
                    paymentMultiplier -= perFlavorWeight;
                    result.deductions.Add($"Missing desired flavor: {desired.ToCamelCaseString()} (-{perFlavorWeight * 100:F0}%)");
                }
            }
        }

        // --- Unwanted effects: deduct for each present unwanted effect (non-100 dislikes) ---
        if (order.unwantedEffects != null && product.Effects != null)
        {
            float perUnwantedWeight = 0.10f;
            foreach (var unwanted in order.unwantedEffects)
            {
                if (product.Effects.Contains(unwanted))
                {
                    paymentMultiplier -= perUnwantedWeight;
                    result.deductions.Add($"Unwanted effect present: {unwanted.ToCamelCaseString()} (-{perUnwantedWeight * 100:F0}%)");
                }
            }
        }

        // --- Unwanted flavors: deduct for each present unwanted flavor (non-100 dislikes) ---
        if (order.unwantedFlavors != null && product.Flavors != null)
        {
            float perUnwantedWeight = 0.10f;
            foreach (var unwanted in order.unwantedFlavors)
            {
                if (product.Flavors.Contains(unwanted))
                {
                    paymentMultiplier -= perUnwantedWeight;
                    result.deductions.Add($"Unwanted flavor present: {unwanted.ToCamelCaseString()} (-{perUnwantedWeight * 100:F0}%)");
                }
            }
        }

        // --- THC out of range ---
        if (product.Thc < order.thcRange.x || product.Thc > order.thcRange.y)
        {
            paymentMultiplier -= 0.10f;
            result.deductions.Add($"THC {product.Thc}% outside preferred range {order.thcRange.x:F0}%-{order.thcRange.y:F0}% (-10%)");
        }

        // --- CBD out of range ---
        if (product.Cbd < order.cbdRange.x || product.Cbd > order.cbdRange.y)
        {
            paymentMultiplier -= 0.10f;
            result.deductions.Add($"CBD {product.Cbd}% outside preferred range {order.cbdRange.x:F0}%-{order.cbdRange.y:F0}% (-10%)");
        }

        // clamp multiplier so the customer always pays at least a small amount if they accept
        paymentMultiplier = Mathf.Clamp(paymentMultiplier, 0.1f, 1f);
        result.payment = Mathf.Max(1, Mathf.RoundToInt(money * paymentMultiplier));

        return result;
    }

    /// <summary>
    /// Returns a readable summary of the customer's order for debug/inspector display.
    /// </summary>
    public string OrderSummary()
    {
        if (order == null) return "No order";

        var sb = new StringBuilder();
        sb.AppendLine($"Category: {order.Category.ToCamelCaseString()}");
        sb.AppendLine($"Budget: ${money}");
        if (order.desiredEffects.Count > 0)
            sb.AppendLine($"Desired Effects: {string.Join(", ", order.desiredEffects)}");
        if (order.desiredFlavors.Count > 0)
            sb.AppendLine($"Desired Flavors: {string.Join(", ", order.desiredFlavors)}");
        if (order.unwantedEffects.Count > 0)
            sb.AppendLine($"Unwanted Effects: {string.Join(", ", order.unwantedEffects)}");
        if (order.unwantedFlavors.Count > 0)
            sb.AppendLine($"Unwanted Flavors: {string.Join(", ", order.unwantedFlavors)}");
        sb.AppendLine($"THC: {order.thcRange.x:F0}%-{order.thcRange.y:F0}%");
        sb.AppendLine($"CBD: {order.cbdRange.x:F0}%-{order.cbdRange.y:F0}%");

        return sb.ToString();
    }
}
