using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Services.ResourcesManagement
{
    public interface IAddressablesService
    {
        UniTask<T> LoadAssetAsync<T>(string address) where T : Object;

        UniTask<GameObject> InstantiatePrefabAsync(
            string address, 
            Vector3 position, 
            Quaternion rotation,
            Transform parent = null);

        void ReleaseResource(string address);
    }
}