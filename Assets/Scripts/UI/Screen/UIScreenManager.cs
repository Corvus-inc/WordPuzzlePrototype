using Core.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Screen
{
    public class UIScreenManager : IUIScreenManager
    {
        private readonly DiContainer _container;
        
        private readonly Transform _uiRoot;
        private readonly GameObject _mainBackground;

        private readonly GameObject _mainMenuPrefab;
        private readonly GameObject _gameUIPrefab;

        private GameObject _mainMenu;
        private GameObject _gameUI;
        
        private readonly IResourceManager _resourceManager;

        public UIScreenManager(
            DiContainer container,
            Transform uiRoot,
            GameObject mainMenuPrefab,
            GameObject gameUIPrefab,
            GameObject mainBackgroundPrefab,
            IResourceManager resourceManager)
        {
            _container = container;
            
            _uiRoot = uiRoot;
            
            _mainMenuPrefab = mainMenuPrefab;
            _gameUIPrefab = gameUIPrefab;
            _mainBackground = _container.InstantiatePrefab(mainBackgroundPrefab);
            _resourceManager = resourceManager;
        }

        public void ShowMainMenu()
        {
            UnloadAll();
            SetupBackground(0);
            _mainMenu = _container.InstantiatePrefab(_mainMenuPrefab, _uiRoot);
        }

        public void ShowGameUI()
        {
            UnloadAll();
            _gameUI = _container.InstantiatePrefab(_gameUIPrefab, _uiRoot);
        }

        private void UnloadAll()
        {
            if (_mainMenu != null) Object.Destroy(_mainMenu);
            if (_gameUI != null) Object.Destroy(_gameUI);
        }
        
        public async UniTask SetupBackground(int spriteId)
        {
            if (_mainBackground != null)
            {
                var spritePath = $"UI/Background/Back{spriteId}";
                var sprite = await _resourceManager.LoadAsync<Sprite>(spritePath);
                
                if (sprite == null)
                {
                    Debug.LogError($"Failed to load sprite: {spritePath}");
                    return;
                }
                
                _mainBackground.GetComponentInChildren<Image>().sprite = sprite;
            }
            else
            {
                Debug.LogError("Main background is not set.");
            }
        }
    }

}