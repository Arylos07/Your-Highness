using UnityEngine;
using Budtender.Traits;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "TasteProfile", menuName = "Budtender/Taste Profile")]
public class TasteProfile : SerializedScriptableObject
{
    public Dictionary<Category, float> categoryPreference = new Dictionary<Category, float>();
    public Dictionary<Effects, float> effectsPreference = new Dictionary<Effects, float>();
    public Dictionary<Flavors, float> flavorsPreference = new Dictionary<Flavors, float>();

    [MinMaxSlider(0,100, true)]
    public Vector2 thcPreference = new Vector2(10, 25);
    [MinMaxSlider(0,100, true)]
    public Vector2 cbdPreference = new Vector2(10, 25);

    //one idea for the future is a median preference
    //It will be randomly selected from the ranges, and if a 
    // product has exactly that value, it gets a bonus
    //This would be in the background to add some variability to preferences, rather than
    // an explicit mechanic. The idea would be that if a player has two products that fit the profile,
    // but one has a THC value closer to the median preference, it might be rated slightly higher.
    //This would encourage players to experiment with different strains and products.
    public int getThcMedian()
    {
        return Random.Range((int)thcPreference.x, (int)thcPreference.y + 1);
    }

    public int getCbdMedian()
    {
        return Random.Range((int)cbdPreference.x, (int)cbdPreference.y + 1);
    }
}
