using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Sirenix.Serialization;

public class Spawner : MonoBehaviour
{
    public List<FlowerTemplate> flowerTemplates; // List of flower templates to spawn from
    Flower newFlower;

    //We need to remove this when we flesh the system out.
    //We can't use this in the new input system.
    void OnMouseDown()
    {
        // Existing functionality
        if (flowerTemplates != null && flowerTemplates.Count > 0)
        {
            // Pick a random template from the list
            FlowerTemplate randomTemplate = flowerTemplates[Random.Range(0, flowerTemplates.Count)];

            // Instantiate a new Flower from the template
            newFlower = new Flower(randomTemplate);

            // Create a FlowerSlot with the new Flower
            FlowerSlot newSlot = new FlowerSlot(newFlower, 1);

            // Add the FlowerSlot to the GameManager's FlowerInventory
            GameManager.Instance.FlowerInventory.Add(newSlot);

            Debug.Log($"Added {newFlower.Name} to the GameManager's inventory.");
        }
        else
        {
            Debug.LogWarning("No flower templates available in the Spawner.");
        }
    }
}
