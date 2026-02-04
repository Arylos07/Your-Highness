using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static Traits; // Assuming Traits is in the same namespace or imported correctly

public class Flower
{
    public string Name { get; private set; }
    public Category Category { get; private set; }
    public int Thc { get; private set; }
    public int Cbd { get; private set; }
    public List<Effects> Effects { get; private set; }
    public List<Flavors> Flavors { get; private set; }

    [SerializeField, TextArea(1, 30)] 
    public string flavorText; // not public, use Tooltip()

    public Flower()
    {
        Name = "Unnamed Flower";
        Category = Category.Hybrid;
        Thc = 0;
        Cbd = 0;
        Effects = new List<Effects>();
        Flavors = new List<Flavors>();
        flavorText = string.Empty;
    }

    public Flower(
    string name, Category category, int thc, int cbd,
    List<Effects> effects, List<Flavors> flavors, string flavorText = "")
    {
        Name = name;
        Category = category;
        Thc = thc;
        Cbd = cbd;
        Effects = new List<Effects>(effects);
        Flavors = new List<Flavors>(flavors);
        flavorText = flavorText ?? string.Empty;
    }

    // Constructor: creates a Flower instance from a FlowerTemplate
    public Flower(FlowerTemplate template)
    {
        Name = template.names.Count > 0 ? template.names[Random.Range(0, template.names.Count)] : "Unnamed Flower";
        Category = template.category;
        Thc = Random.Range(template.thcMin, template.thcMax + 1);
        Cbd = Random.Range(template.cbdMin, template.cbdMax + 1);

        Effects = new List<Effects>(template.effects);
        Shuffle(Effects);
        if (Effects.Count > 3) Effects = Effects.GetRange(0, 3);

        Flavors = new List<Flavors>(template.flavors);
        Shuffle(Flavors);
        if (Flavors.Count > 3) Flavors = Flavors.GetRange(0, 3);

        // Copy and prepare flavor text for dynamic replacement
        flavorText = string.IsNullOrEmpty(template.FlavorText) ? string.Empty : template.FlavorText;
    }

    // Fisher-Yates shuffle for lists
    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    public Seed ConvertToSeed()
    {
        Seed seed = new Seed(this);
        return seed;
    }

    // Tooltip: shows values and traits at a glance
    public virtual string Tooltip(bool includeFlavorText = false)
    {
        string cbdDisplay = Cbd == 0 ? "<1%" : $"{Cbd}%";
        var sb = new StringBuilder();
        sb.AppendLine($"Name: {Name}");
        sb.AppendLine($"Category: {Category.ToCamelCaseString()}");
        sb.AppendLine($"THC: {Thc}%");
        sb.AppendLine($"CBD: {cbdDisplay}");
        sb.AppendLine($"Effects: {string.Join(", ", Effects)}");
        sb.AppendLine($"Flavors: {string.Join(", ", Flavors)}");
        if (includeFlavorText && !string.IsNullOrEmpty(flavorText))
        {
            sb.AppendLine();
            sb.AppendLine(flavorText.Replace("{NAME}", Name));
        }

        return sb.ToString();
    }

    public string EncodeTrats()
    {
        TraitsContainer container = new TraitsContainer
        {
            Category = this.Category,
            Effects = this.Effects,
            Flavors = this.Flavors
        };
        return Traits.EncodeTraits(container);
    }
}
