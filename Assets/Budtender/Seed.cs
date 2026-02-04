using System.Text;
using UnityEngine;
using static Traits;

public class Seed : FlowerTemplate
{
    public string Name;
    //main constructor building a new seed from an existing flower
    // this is the same as extracting seeds from a bud for regrowing
    public Seed(Flower flower)
    {
        Name = flower.Name; //not a fan, but technically works
        category = flower.Category;

        //I'm not a fan of using this for noise as its range is 0-0.9. That can make a seed that randomly doubles
        // or does nothing for no reason. We'll want to create a noise generator later.
        thcMin = Mathf.FloorToInt(flower.Thc - (Random.value * 10));
        thcMax = Mathf.FloorToInt(flower.Thc + (Random.value * 10));

        cbdMin = Mathf.FloorToInt(flower.Cbd - (Random.value * 10));
        cbdMax = Mathf.FloorToInt(flower.Cbd + (Random.value * 10));

        //maybe write an algorithm where one of these can randomly change slightly?
        // To be fair, I think that's better suited in rewriting these
        // so that each has their own percentage to do so.
        effects = flower.Effects;
        flavors = flower.Flavors;

        FlavorText = flower.flavorText;
    }

    // Tooltip: shows values and traits at a glance
    public virtual string Tooltip(bool includeFlavorText = false)
    {
        string cbdDisplay = cbdMax == 0 ? "<1%" : $"{cbdMin} - {cbdMax}%";
        string thcDisplay = $"{thcMin} - {thcMax}%";
        var sb = new StringBuilder();
        sb.AppendLine($"Name: {Name}");
        sb.AppendLine($"Category: {category.ToCamelCaseString()}");
        sb.AppendLine($"THC: {thcDisplay}%");
        sb.AppendLine($"CBD: {cbdDisplay}");
        sb.AppendLine($"Effects: {string.Join(", ", effects)}");
        sb.AppendLine($"Flavors: {string.Join(", ", flavors)}");
        if (includeFlavorText && !string.IsNullOrEmpty(FlavorText))
        {
            sb.AppendLine();
            sb.AppendLine(FlavorText.Replace("{NAME}", Name));
        }

        return sb.ToString();
    }
}
