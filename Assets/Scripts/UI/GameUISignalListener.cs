using System;
using Game;
using Game.Signals;
using UI.Popup;
using UI.Screen;
using Zenject;

namespace UI
{
    public class GameUISignalListener : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly IUIScreenManager _uiScreenManager;
        private readonly IUIPopupManager _uiPopupManager;

        public GameUISignalListener(SignalBus signalBus,
            IUIScreenManager uiScreenManager,
            IUIPopupManager uiPopupManager)
        {
            _signalBus = signalBus;
            _uiScreenManager = uiScreenManager;
            _uiPopupManager = uiPopupManager;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<ShowMainMenuSignal>(OnShowMainMenu);
            _signalBus.Subscribe<ShowGameUISignal>(OnShowGameUI);
            _signalBus.Subscribe<ShowVictorySignal>(OnShowVictory);
        }

        public void Dispose()
        {
            // Важно отписаться от событий, чтобы избежать утечек
            _signalBus.Unsubscribe<ShowMainMenuSignal>(OnShowMainMenu);
            _signalBus.Unsubscribe<ShowGameUISignal>(OnShowGameUI);
            _signalBus.Unsubscribe<ShowVictorySignal>(OnShowVictory);
        }

        public void OnShowMainMenu()
        {
            _uiScreenManager.ShowMainMenu();
        }

        public void OnShowGameUI()
        {
            _uiScreenManager.ShowGameUI();
        }

        public void OnShowVictory(ShowVictorySignal signal)
        {
            _uiPopupManager.ShowVictoryPopup(signal.Words );
        }
    }
}