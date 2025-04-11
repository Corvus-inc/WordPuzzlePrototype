using System;
using Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private Image mainBackground;
        [SerializeField] private Transform uiRoot;
        [SerializeField] private GameObject mainMenuPrefab;
        [SerializeField] private GameObject gameUIPrefab;

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        public override void InstallBindings()
        {
            Container.Bind<RemoteConfigManager>().AsSingle();
            Container.Bind<LevelListProvider>().AsSingle();
            Container.Bind<LevelService>().AsSingle();
            
            Container.Bind<UIScreenManager>().AsSingle()
                .WithArguments(uiRoot, mainMenuPrefab, gameUIPrefab, mainBackground);
            Container.Bind<PopupController>()
                .AsSingle()
                .WithArguments(uiRoot.GetComponent<Canvas>());
            
            Container.Bind<GameFlowController>().AsSingle();
            Container.BindInterfacesTo<GameInitializer>().AsSingle();
        }
    }
}