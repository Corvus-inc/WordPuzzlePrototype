using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core
{
    public class AddressableResourceManager : IResourceManager 
    {
        public async UniTask<T> LoadAsync<T>(string address) 
        {
            var handle = Addressables.LoadAssetAsync<T>(address);
            return await handle.Task;
        }

        public async UniTask<GameObject> InstantiateAsync(string address) 
        {
            var handle = Addressables.InstantiateAsync(address);
            return await handle.Task;
        }

        public void Release(object asset) 
        {
            Addressables.Release(asset);
        }
    }
}