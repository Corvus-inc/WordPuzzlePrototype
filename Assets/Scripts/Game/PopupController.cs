using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Game
{
    public class PopupController
    {
        private readonly DiContainer _container;
        private readonly Canvas _uiCanvas;
        
        private GameObject _settingsPopup;

        public PopupController(DiContainer container, Canvas uiCanvas)
        {
            _container = container;
            _uiCanvas = uiCanvas;
        }

        public async void ShowVictoryPopup()
        {
            const string popupKey = "UI/PopupVictoryWindow";
           
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(popupKey);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var prefab = handle.Result;
                var instance = _container.InstantiatePrefab(prefab, _uiCanvas.transform);
                instance.SetActive(true);
            }
            else
            {
                Debug.LogError($"[PopupController] Failed to load popup: {popupKey}");
            }
        }

        public async void ShowSettingsPopup()
        {
            if (_settingsPopup != null)
            {
                _settingsPopup.SetActive(true);
                return;
            }
            
            const string popupKey = "UI/PopupSettingsWindow";

            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(popupKey);
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var prefab = handle.Result;
                var instance = _container.InstantiatePrefab(prefab, _uiCanvas.transform);
                instance.SetActive(true);

                _settingsPopup = instance;
            }
            else
            {
                Debug.LogError($"[PopupController] Failed to load popup: {popupKey}");
            }
        }
    }
}