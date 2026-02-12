using Budtender.Templates;
using QFSW.QC;
using System.IO;
using System.Threading.Tasks; // Add this for Task support
using UnityEngine;
using Budtender.Orders;
using Budtender;
using System;
using System.Text;

namespace DebugTools.Commands
{
    public class Commands
    {
        [Command]
        public static async Task AddRandomFlower(string templateName, int amount)
        {
            Product flower = await TemplateManager.GenerateRandomFlowerAsync(templateName);
            GameManager.Instance.ProductInventory.Add(new ProductSlot(flower, amount));
            Debug.Log($"Added flower: {amount}x {flower.Name}");
        }

        [Command]
        public static async Task AddRandomFlowers(int flowerTypes, int numMin, int numMax)
        {
            for (int i = 0; i < flowerTypes; i++)
            {
                Product flower = await TemplateManager.GenerateRandomFlowerAsync();
                int amount = UnityEngine.Random.Range(numMin, numMax + 1);
                GameManager.Instance.ProductInventory.Add(new ProductSlot(flower, amount));
                Debug.Log($"Added flower: {amount}x {flower.Name}");
            }
        }

        [Command]
        public static async Task InitInventory()
        {
            int flowers = UnityEngine.Random.Range(1, 10);
            int flowerSlots = UnityEngine.Random.Range(5, 10);
            for (int i = 0; i < flowerSlots; i++)
            {
                Product flower = await TemplateManager.GenerateRandomFlowerAsync();
                int amount = UnityEngine.Random.Range(1, flowers + 1);
                GameManager.Instance.ProductInventory.Add(new ProductSlot(flower, amount));
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

        [Command]
        public static void GetInventorySummary()
        {
            InventorySummary summary = OrderManager.Instance.SummarizeInventory();
            Debug.Log(summary.ToString());
        }

        [Command]
        public static async Task AddProduct(string _type, string template, int amount)
        {
            if (!System.Enum.TryParse(_type, true, out ProductType productType))
            {
                string validTypes = string.Join(", ", System.Enum.GetNames(typeof(ProductType)));
                throw new System.ArgumentException($"Invalid product type: '{_type}'. Valid types: {validTypes}");
            }

            Product product = await TemplateManager.GenerateRandomFlowerAsync(template);
            product.ChangeProductType(productType);
            GameManager.Instance.ProductInventory.Add(new ProductSlot(product, amount));
        }

        [Command]
        public static void ClearInventory(bool removeSeeds = false)
        {
            int products = GameManager.Instance.ProductInventory.Count;
            int seeds = GameManager.Instance.SeedInventory.Count;

            GameManager.Instance.ProductInventory = new System.Collections.Generic.List<ProductSlot>();

            Debug.Log($"Removed {products} products from inventory.");

            if (removeSeeds)
            {
                GameManager.Instance.SeedInventory = new System.Collections.Generic.List<SeedSlot>();

                Debug.Log($"Removed {seeds} seeds from inventory.");
            }
        }

        // Dumps a quick text snapshot of the player's flower/product inventory.
        // Usage from QC console:
        //  - DumpInventory()
        //  - DumpInventory true            -> also writes to Application.persistentDataPath/InventoryDump.txt
        //  - DumpInventory true "myfile.txt"
        [Command]
        public static void DumpInventory(bool writeToFile = false, string fileName = "InventoryDump.txt")
        {
            var gm = GameManager.Instance;
            if (gm == null)
            {
                Debug.LogError("GameManager.Instance is null. Cannot dump inventory.");
                return;
            }

            var inventory = gm.ProductInventory;
            if (inventory == null || inventory.Count == 0)
            {
                Debug.Log("Product inventory is empty.");
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine($"Inventory Snapshot - {DateTime.Now:MM/dd/yyyy HH:mm:ss}");
            sb.AppendLine($"Total Slots: {inventory.Count}");
            sb.AppendLine();

            for (int i = 0; i < inventory.Count; i++)
            {
                var slot = inventory[i];
                if (slot.flower == null || slot.amount == 0)
                {
                    sb.AppendLine($"Slot {i}: (empty)");
                    sb.AppendLine();
                    continue;
                }

                // Use the ProductSlot.ToolTip() to get the product tooltip text
                // This includes product details and will replace any {AMOUNT} tokens if present.
                sb.AppendLine($"Slot {i}: {slot.flower.Name} x{slot.amount}");
                sb.AppendLine(slot.ToolTip());
                sb.AppendLine(new string('-', 40));
            }

            string output = sb.ToString();

            // Log to console (quick snapshot)
            Debug.Log(output);

            if (writeToFile)
            {
                string path = Path.Combine(Application.persistentDataPath, fileName);
                try
                {
                    File.WriteAllText(path, output);
                    Debug.Log($"Inventory dumped to: {path}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to write inventory dump to '{path}': {ex.Message}");
                }
            }
        }
    }
}