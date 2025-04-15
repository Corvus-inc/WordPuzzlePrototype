using Core;
using Core.Interfaces;
using UnityEngine;
using Zenject;

namespace Game.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private AudioSource audioSource;
        
        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        public override void InstallBindings()
        {
            Container.Bind<IResourceManager>().To<AddressableResourceManager>().AsSingle();
            
            Container.Bind<RemoteConfigManager>().AsSingle();
            Container.Bind<LevelListProvider>().AsSingle();
            Container.Bind<LevelService>().AsSingle();
            
            Container.Bind<GameFlowController>().AsSingle();
            Container.BindInterfacesTo<GameInitializer>().AsSingle();
            
            Container.Bind<IMusicManager>().To<MusicManager>().AsSingle()
                .WithArguments(audioSource);
        }
    }
}