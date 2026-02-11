using UnityEngine;
using Sirenix.OdinInspector;
using System;
using Budtender;
public class GrowPlot : MonoBehaviour
{
    public GameManager gameManager;
    public Seed template;
    //private Flower Sapling;
    public GrowthStage Stage = GrowthStage.Empty; // Current growth stage of the plant in this plot
    public int HarvestDays = -1; // -1 is unset, meaning this plot cannot be harvested yet.
    public PlotSpawner spawner; // Reference to the PlotSpawner that will handle spawning the flower when ready

    void Start()
    {
        if (gameManager == null) gameManager = GameManager.Instance;
        gameManager.OnDayAdvanced += OnDayAdvanced; // Subscribe to the day advanced event

        if(template != null) PlantSeedDebug(); // Automatically plant the seed if a template is assigned
    }

    void OnDayAdvanced(DateTime date)
    {
        if (Stage == GrowthStage.Empty) return;

        if ((int)Stage < (int)GrowthStage.Flowering)
        {
            Stage++; // Advance the growth stage
            Debug.Log($"Growth stage advanced to {Stage} for plot with flower {template.name ?? "None"}.");
            if (Stage == GrowthStage.Flowering)
            {
                HarvestDays = UnityEngine.Random.Range(1, 4); // Set a random harvest count between 1 and 3
                if (spawner != null)
                {
                    spawner.flowerTemplate = template; // Assign the flower template to the spawner
                    spawner.isActive = true; // Enable the spawner to allow harvesting
                }
                Debug.Log($"Plot at {transform.position} is ready for harvest with {HarvestDays} flowers.");
            }
        }
        else if (Stage == GrowthStage.Flowering && HarvestDays > 0)
        {
            HarvestDays--; // Decrement the harvest count
        }
        
        //new block so this is called after the above block
        // This is so that we can check if the deduction causes the plant to die. 
        // If we don't do this, then plants get an additional day that they can be harvested, but won't produce flowers.
        if (Stage == GrowthStage.Flowering && HarvestDays == 0)
        {
            Stage = GrowthStage.Dead; // Move to dead stage after all harvests
            // TODO: Implement death/removal logic here
            spawner.isActive = false;
            HarvestDays = -1; // Reset harvest count
            Debug.Log($"Plot at {transform.position} is now dead and needs to be removed.");
        }
    }
    /*
    public void PlantSeed(Flower sapling)
    {
        if (Stage != GrowthStage.Empty) return; // Cannot plant if the plot is not empty
        Sapling = sapling;
        Stage = GrowthStage.Seed; // Set the growth stage to Seed
        HarvestCount = -1;
        if (spawner != null)
        {
            spawner.enabled = false; // Disable spawner until flowering
        }
        Debug.Log($"Planted {Sapling.Name} in the plot.");
    }
    */

    public void PlantSeedDebug()
    {
        if (Stage != GrowthStage.Empty) return; // Cannot plant if the plot is not empty
        Stage = GrowthStage.Seed; // Set the growth stage to Seed
        HarvestDays = -1;
        if (spawner != null)
        {
            spawner.isActive = false; // Disable spawner until flowering
        }
        Debug.Log($"Planted {template.name} in the plot.");
    }
}