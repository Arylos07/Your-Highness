using UnityEngine;
using Budtender.Traits;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using System.Linq;

[CreateAssetMenu(fileName = "TasteProfile", menuName = "Budtender/Taste Profile")]
public class TasteProfile : SerializedScriptableObject
{
    public const int MAX_PREFERENCE_VALUE = 100;

    [Header("PREFERENCES")]
    [InfoBox("How strong the NPC will prefer each trait. The higher the value, the more frequently they will request it. Traits not listed here will not be factored, and the NPC won't care for it. (THIS IS NOT THE SAME AS DISLIKE)")]
    public Dictionary<Category, int> categoryPreference = new Dictionary<Category, int>();
    public Dictionary<Effects, int> effectsPreference = new Dictionary<Effects, int>();
    public Dictionary<Flavors, int> flavorsPreference = new Dictionary<Flavors, int>();

    [HideIf("balancedPreferences")]
    [InfoBox("One or more of the preferences values adds the total up to be inequal to 100. This will skew the math as we want equal distribution across all traits. Run the function below to balance the list of dislikes.", InfoMessageType.Warning)]
    [Button]
    public void RecalculatePreferences()
    {
        categoryPreference = categoryPreference.NormalizeDictionary(MAX_PREFERENCE_VALUE);
        effectsPreference = effectsPreference.NormalizeDictionary(MAX_PREFERENCE_VALUE);
        flavorsPreference = flavorsPreference.NormalizeDictionary(MAX_PREFERENCE_VALUE);
        ValidatePreferences();
    }

    //maybe have the RNG weighted against this? 
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

    //DISLIKES
    [Header("DISLIKES")]
    [InfoBox("How strong the NPC will dislike each trait. The higher the value, the less they will pay for the product if it has these traits. Traits not listed here will not be factored, and the NPC won't mind it.\n\nThe methodology is that if a product has some things an NPC likes, with one or two traits they don't like, they'll pay less and give less reputation; maybe pop a quest flag.\n\nValues of 100 mean the NPC will never buy the product if it has these traits")]
    public Dictionary<Category, int> categoryDislikes = new Dictionary<Category, int>();
    public Dictionary<Effects, int> effectsDislikes = new Dictionary<Effects, int>();
    public Dictionary<Flavors, int> flavorsDislikes = new Dictionary<Flavors, int>();



    [HideIf("balancedDislikes")]
    [InfoBox("One or more of the Dislike values adds the total up to be inequal to 100. This will skew the math as we want equal distribution across all traits. Run the function below to balance the list of dislikes.", InfoMessageType.Warning)]
    [Button]
    void RecalculateDislikes()
    {
        categoryDislikes = categoryDislikes.NormalizeDictionary(MAX_PREFERENCE_VALUE);
        effectsDislikes = effectsDislikes.NormalizeDictionary(MAX_PREFERENCE_VALUE);
        flavorsDislikes = flavorsDislikes.NormalizeDictionary(MAX_PREFERENCE_VALUE);
        ValidateDislikes();
    }


    //improve this validation step. Right now, if no preferences are specified, then it thinks they're imbalanced. To be fair, they are, but it's not a useful warning.
    bool balancedPreferences;

    bool balancedDislikes;

    private void OnValidate()
    {
        ValidatePreferences();
        ValidateDislikes();
    }

    void ValidatePreferences()
    {
        balancedPreferences = (categoryPreference.Count != 0 ? categoryPreference.Values.Sum() == MAX_PREFERENCE_VALUE : true) &&
                      (effectsPreference.Count != 0 ? effectsPreference.Values.Sum() == MAX_PREFERENCE_VALUE : true) &&
                      (flavorsPreference.Count != 0 ? flavorsPreference.Values.Sum() == MAX_PREFERENCE_VALUE : true);
    }

    void ValidateDislikes()
    {
        balancedDislikes = (categoryDislikes.Count != 0 ? categoryDislikes.Values.Sum() == MAX_PREFERENCE_VALUE : true) &&
                    (effectsDislikes.Count != 0 ? effectsDislikes.Values.Sum() == MAX_PREFERENCE_VALUE : true) &&
                    (flavorsDislikes.Count != 0 ? flavorsDislikes.Values.Sum() == MAX_PREFERENCE_VALUE : true);
    }
}
