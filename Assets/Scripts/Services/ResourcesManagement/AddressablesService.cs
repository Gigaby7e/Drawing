using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Services.ResourcesManagement
{
    public class AddressablesService : IAddressablesService
    {
        public async UniTask<T> LoadAssetAsync<T>(string address) where T : Object
        {
            var handle = Addressables.LoadAssetAsync<T>(address);
            await handle.Task;

            return handle.Status == AsyncOperationStatus.Succeeded ? handle.Result : null;
        }

        public async UniTask<GameObject> InstantiatePrefabAsync(string address, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var handle = Addressables.InstantiateAsync(address, position, rotation, parent);
            
            await handle.Task;

            return handle.Status == AsyncOperationStatus.Succeeded ? handle.Result : null;
        }

        public void ReleaseResource(string address) => Addressables.Release(address);
    }
}