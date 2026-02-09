using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Budtender.Templates
{
    public class TemplateManager
    {
        const string FlowerTemplateLabel = "FlowerTemplate";

        //so stupid, but this is the only way that I can make a constant label reference.
        static AssetLabelReference flowerTemplateLabel
        {
            get
            {
                var _label = new AssetLabelReference();
                _label.labelString = FlowerTemplateLabel;
                return _label;
            }
        }

        // Existing callback-based API left intact
        public static void LoadTemplateAsync<T>(string address, System.Action<T> onLoaded) where T : FlowerTemplate
        {
            Addressables.LoadAssetAsync<T>(address).Completed += (AsyncOperationHandle<T> handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    onLoaded?.Invoke(handle.Result);
                }
                else
                {
                    Debug.LogError($"Failed to load template at address: {address}");
                    onLoaded?.Invoke(null);
                }
            };
        }

        public static void UnloadTemplate<T>(T template) where T : FlowerTemplate
        {
            Addressables.Release(template);
        }

        // Async/await version - caller can await this to get a ready Flower instance.
        public static async Task<Flower> GenerateRandomFlowerAsync(string address, bool unloadWhenDone = true)
        {
            var handle = Addressables.LoadAssetAsync<FlowerTemplate>(address);

            FlowerTemplate template = null;
            try
            {
                // Await the underlying Task so we don't return before the load completes.
                template = await handle.Task;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Exception while loading template at address: {address} - {ex}");
            }

            if (handle.Status == AsyncOperationStatus.Succeeded && template != null)
            {
                var flower = new Flower(template);
                if (unloadWhenDone) UnloadTemplate(template);
                return flower;
            }

            Debug.LogError($"Failed to load template at address: {address}");
            return null;
        }

        public static async Task<Flower> GenerateRandomFlowerAsync(bool unloadWhenDone = true)
        {
            // Start loading the resource locations for the label
            AsyncOperationHandle<IList<IResourceLocation>> handle = Addressables.LoadResourceLocationsAsync(flowerTemplateLabel.labelString);

            IList<IResourceLocation> locations = null;
            try
            {
                // Await the task so we don't continue until the locations are available
                locations = await handle.Task;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Exception while loading resource locations for label '{flowerTemplateLabel.labelString}' - {ex}");
            }

            // Only proceed if load succeeded and we have locations
            if (handle.Status == AsyncOperationStatus.Succeeded && locations != null && locations.Count > 0)
            {
                // Pick a random location and capture its primary key (address)
                var selection = locations[Random.Range(0, locations.Count)];
                string selectedAddress = selection.PrimaryKey;

                // Release the locations handle (we only needed the list of addresses)
                if (handle.IsValid()) Addressables.Release(handle);

                // Load the selected asset and return the Flower
                return await GenerateRandomFlowerAsync(selectedAddress, unloadWhenDone);
            }

            if (handle.IsValid()) Addressables.Release(handle);

            Debug.LogError($"Failed to load resource locations for label: {flowerTemplateLabel.labelString}");
            return null;
        }
    }
}