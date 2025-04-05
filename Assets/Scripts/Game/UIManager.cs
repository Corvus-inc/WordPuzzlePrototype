using UnityEngine;
using Zenject;

namespace Game
{
    public class UIManager
    {
        private readonly DiContainer _container;
        
        private readonly Transform _uiRoot;

        private readonly GameObject _mainMenuPrefab;

        private GameObject _mainMenu;

        public UIManager(
            DiContainer container,
            Transform uiRoot,
            GameObject mainMenuPrefab)
        {
            _container = container;
            
            _uiRoot = uiRoot;
            
            _mainMenuPrefab = mainMenuPrefab;
        }

        public void ShowMainMenu()
        {
            UnloadAll();
            _mainMenu = _container.InstantiatePrefab(_mainMenuPrefab, _uiRoot);
        }

        private void UnloadAll()
        {
            if (_mainMenu != null) Object.Destroy(_mainMenu);
        }
    }

}