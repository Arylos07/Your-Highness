using UnityEngine;
using Sirenix.Serialization;
using System;

public class PlotSpawner : MonoBehaviour
{
    public FlowerTemplate flowerTemplate; // Single flower template to spawn from
    private Flower newFlower;
    public bool oncePerDay;
    [SerializeField] public bool isActive = true; // Whether the spawner is active

    private void Start()
    {
        if (oncePerDay)
        {
            GameManager.Instance.OnDayAdvanced += Respawn;
        }
    }

    private void OnMouseDown()
    {
        SpawnFlower(); //debug
    }

    // This method spawns a flower from the assigned template
    public void SpawnFlower()
    {
        if (!isActive)
        {
            Debug.LogWarning("PlotSpawner is inactive. Cannot spawn flower.");
            return;
        }

        if (flowerTemplate != null)
        {
            // Instantiate a new Flower from the template
            newFlower = new Flower(flowerTemplate);

            // Create a FlowerSlot with the new Flower
            FlowerSlot newSlot = new FlowerSlot(newFlower, 1);

            // Add the FlowerSlot to the GameManager's FlowerInventory
            GameManager.Instance.FlowerInventory.Add(newSlot);

            Debug.Log($"Added {newFlower.Name} to the GameManager's inventory.");

            if (oncePerDay) isActive = false;
        }
        else
        {
            Debug.LogWarning("No flower template assigned to the PlotSpawner.");
        }
    }

    // Resets the spawner to its initial state
    public void Respawn(DateTime date)
    {
        isActive = true;
    }

    // Deactivates the spawner (e.g., after a flower is spawned)
    public void DeactivateSpawner()
    {
        isActive = false;
    }
}