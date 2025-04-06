using UnityEngine;
using Zenject;

namespace Game
{
    public class UIManager
    {
        private readonly DiContainer _container;
        
        private readonly Transform _uiRoot;

        private readonly GameObject _mainMenuPrefab;
        private readonly GameObject _gameUIPrefab;

        private GameObject _mainMenu;
        private GameObject _gameUI;

        public UIManager(
            DiContainer container,
            Transform uiRoot,
            GameObject mainMenuPrefab,
            GameObject gameUIPrefab)
        {
            _container = container;
            
            _uiRoot = uiRoot;
            
            _mainMenuPrefab = mainMenuPrefab;
            _gameUIPrefab = gameUIPrefab;
        }

        public void ShowMainMenu()
        {
            UnloadAll();
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
    }

}