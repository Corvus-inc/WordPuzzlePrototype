using Core;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private Transform uiRoot;
        [SerializeField] private GameObject mainMenuPrefab;
        [SerializeField] private GameObject gameUIPrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<RemoteConfigManager>().AsSingle();
            Container.Bind<LevelListProvider>().AsSingle();
            Container.Bind<LevelService>().AsSingle();
            Container.Bind<UIManager>().AsSingle()
                .WithArguments(uiRoot, mainMenuPrefab, gameUIPrefab);
            
            Container.Bind<GameFlowController>().AsSingle();
            Container.BindInterfacesTo<GameInitializer>().AsSingle();
        }
    }
}