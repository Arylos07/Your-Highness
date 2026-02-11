using UnityEngine;
using Sirenix.OdinInspector;
using Budtender;
using Budtender.Traits;

public class BudTester : MonoBehaviour
{
    public FlowerTemplate flowerTemplateA;
    public FlowerTemplate flowerTemplateB;
    public bool printPunnett = false;

    public Product flowerA;
    public Product flowerB;
    public bool printFlowers = false;


    [Header("Traits Encoding Testing")]
    public string encodedTraits;
    public Category testCategory;
    public Effects[] testEffects;
    public Flavors[] testFlavors;

    [Button("Test Breed Templates")]
    public void BreedTemplates()
    {
        if (flowerTemplateA != null && flowerTemplateB != null)
        {
            Product bredFromTemplates = BreedingEngine.Breed(flowerTemplateA, flowerTemplateB, printPunnett);
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
            Product bredFromFlowers = BreedingEngine.Breed(flowerA, flowerB, printFlowers);
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
            Product bredFromTemplates = BreedingEngine.Breed(flowerTemplateA, flowerTemplateB, printPunnett);
            Debug.Log($"Bred Flower from templates:\n{bredFromTemplates.Tooltip(true)}");
        }
        else
        {
            Debug.LogWarning("Flower templates are not assigned for breeding.");
        }
        if (flowerA != null && flowerB != null)
        {
            Product bredFromFlowers = BreedingEngine.Breed(flowerA, flowerB, printFlowers);
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
        flowerA = new Product(flowerTemplateA);
        flowerB = new Product(flowerTemplateB);
        Debug.Log($"Generated Flower A:\n{flowerA.Tooltip(true)}");
        Debug.Log($"Generated Flower B:\n{flowerB.Tooltip(true)}");
    }

    [Button("Test Encode Traits")]
    public void TestEncodeTraits()
    {
        TraitsContainer container = new TraitsContainer
        {
            Category = testCategory,
            Effects = new System.Collections.Generic.List<Effects>(testEffects),
            Flavors = new System.Collections.Generic.List<Flavors>(testFlavors)
        };
        encodedTraits = TraitsManager.EncodeTraits(container);
        Debug.Log($"Encoded Traits: {encodedTraits}");
    }

    [Button("Test Decode Traits")]
    public void TestDecodeTraits()
    {
        TraitsContainer container = TraitsManager.DecodeTraits(encodedTraits);
        testCategory = container.Category;
        testEffects = container.Effects.ToArray();
        testFlavors = container.Flavors.ToArray();
    }
}
