using System.Collections.Generic;
using Core;
using UnityEngine;
using Zenject;

namespace UI.Popup
{
    public class UIPopupManager : IUIPopupManager
    {
        private readonly DiContainer _container;
        private readonly Canvas _uiCanvas;
        private readonly IResourceManager _resourceManager;

        private GameObject _settingsPopup;

        public UIPopupManager(DiContainer container, Canvas uiCanvas, IResourceManager resourceManager)
        {
            _container = container;
            _uiCanvas = uiCanvas;
            _resourceManager = resourceManager;
        }

        public async void ShowVictoryPopup(List<string> solvedWords)
        {
            const string popupKey = "UI/PopupVictoryWindow";

            var prefab = await _resourceManager.LoadAsync<GameObject>(popupKey);
            if (prefab == null)
            {
                Debug.LogError($"Failed to load popup: {popupKey}");
                return;
            }
            
            var instance = _container.InstantiatePrefab(prefab, _uiCanvas.transform);
            instance.SetActive(true);

            if (instance.TryGetComponent(out VictoryPopupWindow victoryPopup))
            {
                victoryPopup.SetWords(solvedWords);
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
            
            var prefab = await _resourceManager.LoadAsync<GameObject>(popupKey);
            if (prefab == null)
            {
                Debug.LogError($"[PopupController] Failed to load popup: {popupKey}");
                return;
            }
            
            var instance = _container.InstantiatePrefab(prefab, _uiCanvas.transform);
            instance.SetActive(true);
            _settingsPopup = instance;
        }
    }
}
