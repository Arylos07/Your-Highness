using UnityEngine;
using static Traits; // Assuming Traits is in the same namespace or imported correctly
using System.Collections.Generic;
using System.ComponentModel;

[CreateAssetMenu(fileName = "FlowerTemplate", menuName = "Scriptable Objects/FlowerTemplate")]
public class FlowerTemplate : ScriptableObject
{
    public List<string> names = new List<string>();
    public Category category;

    // THC and CBD values are stored as integers representing percentages (0-100)
    // This allows for easy manipulation and comparison without needing to convert to floats.
    // Make sure that any time we display this on the UI, that we convert it to a percentage format.
    // We can use ToString for this
    public int thcMin;
    public int thcMax;
    //this is a container that will be replaced later with the strain's final values

    public Vector2 thcRange => new Vector2(thcMin, thcMax);
    public int cbdMin;
    public int cbdMax;
    //this is a container that will be replaced later with the strain's final values
    public Vector2 cbdRange => new Vector2(cbdMin, cbdMax);

    //using generics here lets us replace these with classes if we decide to expand later
    public List<Effects> effects = new List<Effects>();
    public List<Flavors> flavors = new List<Flavors>();

    public string Description
    {
        get
        {
            string cbdDisplay = cbdMin == 0 ? $"<{cbdMax}%" : $"{cbdMin}% - {cbdMax}%";
            return $"Category: {category.ToCamelCaseString()}\n" +
                   $"THC: {thcMin}% - {thcMax}%\n" +
                   $"CBD: {cbdDisplay}\n" +
                   $"Effects: {string.Join(", ", effects)}\n" +
                   $"Flavors: {string.Join(", ", flavors)}";
        }
    }

    public string Overview
    {
        get
        {
            string cbdDisplay = cbdMin == 0 ? $"<{cbdMax}%" : $"{cbdMin}% to {cbdMax}%";
            return $"{category.ToCamelCaseString()} strain with THC content ranging from {thcMin}% to {thcMax}% and CBD content from {cbdDisplay}. " +
                   $"Known for its effects: {string.Join(", ", effects)} and flavors: {string.Join(", ", flavors)}.";
        }
    }

    [TextArea(3, 10)]
    public string FlavorText;
}
