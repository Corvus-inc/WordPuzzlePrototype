using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core
{
    public interface IResourceManager 
    {
        UniTask<T> LoadAsync<T>(string address);
        UniTask<GameObject> InstantiateAsync(string address);
        void Release(object asset);
    }
}