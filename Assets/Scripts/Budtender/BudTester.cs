using UnityEngine;
using Sirenix.OdinInspector;
using Budtender;

public class BudTester : MonoBehaviour
{
    public FlowerTemplate flowerTemplateA;
    public FlowerTemplate flowerTemplateB;
    public bool printPunnett = false;

    public Flower flowerA;
    public Flower flowerB;
    public bool printFlowers = false;


    [Header("Traits Encoding Testing")]
    public string encodedTraits;
    public Traits.Category testCategory;
    public Traits.Effects[] testEffects;
    public Traits.Flavors[] testFlavors;

    [Button("Test Breed Templates")]
    public void BreedTemplates()
    {
        if (flowerTemplateA != null && flowerTemplateB != null)
        {
            Flower bredFromTemplates = BreedingEngine.Breed(flowerTemplateA, flowerTemplateB, printPunnett);
            Debug.Log($"Bred Flower from templates:\n{bredFromTemplates.Tooltip(true)}");
        }
        else
        {
            Debug.LogWarning("Flower templates are not assigned for breeding.");
        }
    }

    [Button("Test Breed Flowers")]
    public void BreedFlowers()
    {
        if (flowerA != null && flowerB != null)
        {
            Flower bredFromFlowers = BreedingEngine.Breed(flowerA, flowerB, printFlowers);
            Debug.Log($"Bred Flower from flowers:\n{bredFromFlowers.Tooltip(true)}");
        }
        else
        {
            Debug.LogWarning("Flowers are not assigned for breeding.");
        }
    }

    [Button("Test Breed Mixed")]
    public void BreedMixed()
    {
        if (flowerTemplateA != null && flowerTemplateB != null)
        {
            Flower bredFromTemplates = BreedingEngine.Breed(flowerTemplateA, flowerTemplateB, printPunnett);
            Debug.Log($"Bred Flower from templates:\n{bredFromTemplates.Tooltip(true)}");
        }
        else
        {
            Debug.LogWarning("Flower templates are not assigned for breeding.");
        }
        if (flowerA != null && flowerB != null)
        {
            Flower bredFromFlowers = BreedingEngine.Breed(flowerA, flowerB, printFlowers);
            Debug.Log($"Bred Flower from flowers:\n{bredFromFlowers.Tooltip(true)}");
        }
        else
        {
            Debug.LogWarning("Flowers are not assigned for breeding.");
        }
    }

    [Button("Generate Flowers From Templates")]
    public void Generate()
    {
        flowerA = new Flower(flowerTemplateA);
        flowerB = new Flower(flowerTemplateB);
        Debug.Log($"Generated Flower A:\n{flowerA.Tooltip(true)}");
        Debug.Log($"Generated Flower B:\n{flowerB.Tooltip(true)}");
    }

    [Button("Test Encode Traits")]
    public void TestEncodeTraits()
    {
        TraitsContainer container = new TraitsContainer
        {
            Category = testCategory,
            Effects = new System.Collections.Generic.List<Traits.Effects>(testEffects),
            Flavors = new System.Collections.Generic.List<Traits.Flavors>(testFlavors)
        };
        encodedTraits = Traits.EncodeTraits(container);
        Debug.Log($"Encoded Traits: {encodedTraits}");
    }

    [Button("Test Decode Traits")]
    public void TestDecodeTraits()
    {
        TraitsContainer container = Traits.DecodeTraits(encodedTraits);
        testCategory = container.Category;
        testEffects = container.Effects.ToArray();
        testFlavors = container.Flavors.ToArray();
    }
}
