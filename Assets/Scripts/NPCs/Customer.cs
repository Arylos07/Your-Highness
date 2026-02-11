using Budtender.Orders;
using UnityEngine;

[System.Serializable]
public class Customer
{
    public Order order;
    public int money;
    public TasteProfile tasteProfile; //may not be needed, but let's include it

    //ignores the current inventory summary
    public Customer(NpcTemplate template)
    {
        tasteProfile = template.tasteProfile;
        order = new Order(tasteProfile);
        money = Random.Range(template.moneyRange.x, template.moneyRange.y + 1);
    }

    public Customer(NpcTemplate template, InventorySummary inventorySummary)
    {
        tasteProfile = template.tasteProfile;
        order = new Order(tasteProfile, inventorySummary);
        money = Random.Range(template.moneyRange.x, template.moneyRange.y + 1);
    }
}
