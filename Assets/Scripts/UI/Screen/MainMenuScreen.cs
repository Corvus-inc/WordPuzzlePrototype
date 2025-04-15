using Core.Interfaces;
using Game;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Screen
{
    public class MainMenuScreen : MonoBehaviour
    {
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _settingsButton;

        private LevelService _levelService;
        private GameFlowController _gameFlowController;
        private IUIPopupManager _uiPopupManager;

        [Inject]
        public void Construct(LevelService levelService, 
            GameFlowController gameFlowController,
            IUIPopupManager iuiPopupManager)
        {
            _levelService = levelService;
            _gameFlowController = gameFlowController;
            _uiPopupManager = iuiPopupManager;
        }

        private void Start()
        {
            _playButton.onClick.AddListener(OnPlayClicked);
            _settingsButton.onClick.AddListener(OnSettingsClicked);
        }

        private void OnPlayClicked()
        {
            var level = _levelService.GetCurrentLevel();
            Debug.Log($"[MainMenu] Play clicked. Current level ID: {level?.id}");
            _gameFlowController.StartGame();
        }

        private void OnSettingsClicked()
        {
            _uiPopupManager.ShowSettingsPopup();
        }
    }
}