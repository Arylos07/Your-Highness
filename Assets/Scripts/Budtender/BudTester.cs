using UnityEngine;
using Sirenix.OdinInspector;

public class BudTester : MonoBehaviour
{
    public FlowerTemplate flowerTemplateA;
    public FlowerTemplate flowerTemplateB;
    public bool printPunnett = false;

    public Flower flowerA;
    public Flower flowerB;
    public bool printFlowers = false;

    [Button("Test Breed Templates")]
    public void BreedTemplates()
    {
        if (flowerTemplateA != null && flowerTemplateB != null)
        {
            Flower bredFromTemplates = Budtender.Breed(flowerTemplateA, flowerTemplateB, printPunnett);
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
            Flower bredFromFlowers = Budtender.Breed(flowerA, flowerB, printFlowers);
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
            Flower bredFromTemplates = Budtender.Breed(flowerTemplateA, flowerTemplateB, printPunnett);
            Debug.Log($"Bred Flower from templates:\n{bredFromTemplates.Tooltip(true)}");
        }
        else
        {
            Debug.LogWarning("Flower templates are not assigned for breeding.");
        }
        if (flowerA != null && flowerB != null)
        {
            Flower bredFromFlowers = Budtender.Breed(flowerA, flowerB, printFlowers);
            Debug.Log($"Bred Flower from flowers:\n{bredFromFlowers.Tooltip(true)}");
        }
        else
        {
            Debug.LogWarning("Flowers are not assigned for breeding.");
        }
    }
}
