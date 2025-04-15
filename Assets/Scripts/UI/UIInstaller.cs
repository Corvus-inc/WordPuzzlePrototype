using Game;
using Game.Signals;
using UI.Popup;
using UI.Screen;
using UnityEngine;
using Zenject;

namespace UI
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private Canvas uiRoot;
        [SerializeField] private GameObject gameUIPrefab;
        [SerializeField] private GameObject mainMenuPrefab;
        [SerializeField] private GameObject backgroundCanvasPrefab;

        public override void InstallBindings()
        {
            Container.BindSignal<ShowMainMenuSignal>()
                .ToMethod<GameUISignalListener>(listener => listener.OnShowMainMenu)
                .FromResolve();

            Container.BindSignal<ShowGameUISignal>()
                .ToMethod<GameUISignalListener>(listener => listener.OnShowGameUI)
                .FromResolve();

            Container.BindSignal<ShowVictorySignal>()
                .ToMethod<GameUISignalListener>(listener => listener.OnShowVictory)
                .FromResolve();

            // Регистрируем сам listener
            Container.Bind<GameUISignalListener>().AsSingle();
            
            
            Container.Bind<IUIScreenManager>().To<UIScreenManager>().AsSingle()
                .WithArguments(uiRoot.transform, mainMenuPrefab, gameUIPrefab, backgroundCanvasPrefab);
            
            Container.Bind<IUIPopupManager>().To<UIPopupManager>().AsSingle()
                .WithArguments(uiRoot);
        }
    }
}
