using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Budtender.Templates
{
    public class TemplateManager
    {
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
    }
}