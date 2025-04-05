using Core;
using UnityEngine;
using Zenject;

namespace Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<RemoteConfigManager>().AsSingle();
            Container.Bind<LevelListProvider>().AsSingle();
            Container.Bind<LevelService>().AsSingle();
            Container.BindInterfacesTo<GameInitializer>().AsSingle();
            Debug.Log("GameInitializer: initialized");
        }
    }
}