using UnityEngine;
using System.Linq;

public class GrowManager : MonoBehaviour
{
    public GameManager gameManager;

    public GrowPlot[] growPlots; // Array of GrowPlot references to manage multiple plots

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameManager == null) gameManager = GameManager.Instance;
    }

    // Adds a GrowPlot to the growPlots array
    public void AddGrowPlot(GrowPlot plot)
    {
        if (plot == null || growPlots.Contains(plot)) return; // Avoid duplicates or null entries
        var newGrowPlots = new GrowPlot[growPlots.Length + 1];
        growPlots.CopyTo(newGrowPlots, 0);
        newGrowPlots[growPlots.Length] = plot;
        growPlots = newGrowPlots;
    }

    // Removes a GrowPlot from the growPlots array
    public void RemoveGrowPlot(GrowPlot plot)
    {
        if (plot == null || !growPlots.Contains(plot)) return; // Ensure the plot exists in the array
        growPlots = growPlots.Where(p => p != plot).ToArray();
    }
}

public enum GrowthStage
{
    Empty,
    Seed, //just planted
    Sapling,
    Vegetative,
    Flowering, //harvestable
    Dead
}