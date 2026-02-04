using UnityEngine;
using QFSW.QC;
using System.IO;
using Budtender.Templates;
using System.Threading.Tasks; // Add this for Task support

namespace DebugTools.Commands
{
    public class Commands
    {
        [Command]
        public static async Task AddRandomFlower(string templateName, int amount)
        {
            Flower flower = await TemplateManager.GenerateRandomFlowerAsync(templateName);
            //Debug.Log($"Generated flower: {flower.Name}");
            GameManager.Instance.FlowerInventory.Add(new FlowerSlot(flower, amount));
            Debug.Log($"Added flower: {amount}x {flower.Name}");
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