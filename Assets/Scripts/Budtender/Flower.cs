using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static Traits; // Assuming Traits is in the same namespace or imported correctly

public class Flower
{
    public string Name { get; private set; }
    public Category Category { get; private set; }
    public int ThcMin { get; private set; }
    public int ThcMax { get; private set; }
    public Vector2 ThcRange => new Vector2(ThcMin, ThcMax);
    public int CbdMin { get; private set; }
    public int CbdMax { get; private set; }
    public Vector2 CbdRange => new Vector2(CbdMin, CbdMax);
    public List<Effects> Effects { get; private set; }
    public List<Flavors> Flavors { get; private set; }

    [SerializeField, TextArea(1, 30)] 
    protected string flavorText; // not public, use Tooltip()

    // Constructor: creates a Flower instance from a FlowerTemplate
    public Flower(FlowerTemplate template)
    {
        Name = template.names.Count > 0 ? template.names[Random.Range(0, template.names.Count)] : "Unnamed Flower";
        Category = template.category;
        ThcMin = template.thcMin;
        ThcMax = template.thcMax;
        CbdMin = template.cbdMin;
        CbdMax = template.cbdMax;
        Effects = new List<Effects>(template.effects);
        Flavors = new List<Flavors>(template.flavors);

        // Copy and prepare flavor text for dynamic replacement
        flavorText = string.IsNullOrEmpty(template.FlavorText) ? string.Empty : template.FlavorText;
    }

    public virtual string Tooltip()
    {
        StringBuilder tip = new StringBuilder(flavorText);

        //The Item name is only selected when the flower is instantiated, so we can only replace it here; not the template
        tip.Replace("{NAME}", Name);
        // Add more replacements here as needed in the future

        return tip.ToString();
    }
}
