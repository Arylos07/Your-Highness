using Budtender.Templates;
using QFSW.QC;
using System.IO;
using System.Threading.Tasks; // Add this for Task support
using UnityEngine;

namespace DebugTools.Commands
{
    public class Commands
    {
        [Command]
        public static async Task AddRandomFlower(string templateName, int amount)
        {
            Flower flower = await TemplateManager.GenerateRandomFlowerAsync(templateName);
            GameManager.Instance.FlowerInventory.Add(new FlowerSlot(flower, amount));
            Debug.Log($"Added flower: {amount}x {flower.Name}");
        }

        [Command]
        public static async Task AddRandomFlowers(int flowerTypes, int numMin, int numMax)
        {
            for (int i = 0; i < flowerTypes; i++)
            {
                Flower flower = await TemplateManager.GenerateRandomFlowerAsync();
                int amount = Random.Range(numMin, numMax + 1);
                GameManager.Instance.FlowerInventory.Add(new FlowerSlot(flower, amount));
                Debug.Log($"Added flower: {amount}x {flower.Name}");
            }
        }

        [Command]
        public static async Task InitInventory()
        {
            int flowers = Random.Range(1, 10);
            int flowerSlots = Random.Range(5, 10);
            for (int i = 0; i < flowerSlots; i++)
            {
                Flower flower = await TemplateManager.GenerateRandomFlowerAsync();
                int amount = Random.Range(1, flowers + 1);
                GameManager.Instance.FlowerInventory.Add(new FlowerSlot(flower, amount));
                Debug.Log($"Added flower: {amount}x {flower.Name}");
            }
        }

        [Command]
        public static void ModMoney(int amount)
        {
            GameManager.Instance.Money += amount;
            //creature comfort for logging
            string _mod = amount > 0 ? "Added" : "Removed";
            Debug.Log($"{_mod} {amount}");
        }
    }
}