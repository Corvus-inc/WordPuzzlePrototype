using Core.Interfaces;
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
            Container.Bind<IUIScreenManager>().To<UIScreenManager>().AsSingle()
                .WithArguments(uiRoot.transform, mainMenuPrefab, gameUIPrefab, backgroundCanvasPrefab);
            
            Container.Bind<IUIPopupManager>().To<UIPopupManager>().AsSingle()
                .WithArguments(uiRoot);
        }
    }
}
