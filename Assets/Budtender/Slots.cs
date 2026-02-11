using System;
using System.Text;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Budtender;

[Serializable]
public struct FlowerSlot
{
    [OdinSerialize] public Product flower;
    [OdinSerialize] public int amount;

    // Constructor
    public FlowerSlot(Product flower, int amount = 1)
    {
        this.flower = flower;
        this.amount = amount;
    }

    // Decrease the amount, returns the actual amount decreased
    public int DecreaseAmount(int reduceBy)
    {
        int limit = Mathf.Clamp(reduceBy, 0, amount);
        amount -= limit;
        return limit;
    }

    // Increase the amount, returns the actual amount increased
    // You may want to add a maxStack property to Flower if needed
    public int IncreaseAmount(int increaseBy, int maxStack = 99)
    {
        int limit = Mathf.Clamp(increaseBy, 0, maxStack - amount);
        amount += limit;
        return limit;
    }

    // Tooltip
    public string ToolTip()
    {
        if (amount == 0 || flower == null) return string.Empty;

        StringBuilder tip = new StringBuilder(flower.Tooltip());
        tip.Replace("{AMOUNT}", amount.ToString());
        return tip.ToString();
    }
}

public struct SeedSlot
{
    [OdinSerialize] public Seed seed;
    [OdinSerialize] public int amount;

    // Constructor
    public SeedSlot(Seed seed, int amount = 1)
    {
        this.seed = seed;
        this.amount = amount;
    }

    // Decrease the amount, returns the actual amount decreased
    public int DecreaseAmount(int reduceBy)
    {
        int limit = Mathf.Clamp(reduceBy, 0, amount);
        amount -= limit;
        return limit;
    }

    // Increase the amount, returns the actual amount increased
    // You may want to add a maxStack property to Flower if needed
    public int IncreaseAmount(int increaseBy, int maxStack = 99)
    {
        int limit = Mathf.Clamp(increaseBy, 0, maxStack - amount);
        amount += limit;
        return limit;
    }

    // Tooltip
    public string ToolTip()
    {
        if (amount == 0 || seed == null) return string.Empty;

        StringBuilder tip = new StringBuilder(seed.Tooltip());
        tip.Replace("{AMOUNT}", amount.ToString());
        return tip.ToString();
    }
}