using UnityEngine;

namespace Game
{
    public class GameFlowController
    {
        private readonly UIManager _uiManager;
        private readonly PopupController _popupController;
        private readonly LevelService _levelService;

        private SceneState _currentState;

        public GameFlowController(
            UIManager uiManager,
            PopupController popupController,
            LevelService levelService)
        {
            _uiManager = uiManager;
            _currentState = SceneState.Menu;
            _popupController = popupController;
            _levelService = levelService;
        }

        public void GoToMenu()
        {
            _currentState = SceneState.Menu;
            _uiManager.ShowMainMenu();
        }
        
        public void StartGame()
        {
            _currentState = SceneState.Game;
            _uiManager.ShowGameUI();
        }

        public void ShowVictory()
        {
            _popupController.ShowVictoryPopup();
        }

        public void LoadNextLevel()
        {
            bool hasNext = _levelService.TryAdvanceLevel();
            if (hasNext)
            {
                _uiManager.ShowGameUI();
            }
            else
            {
                Debug.Log("No more levels!");
                GoToMenu(); // или остаёмся в победном окне
            }
        }
    }

}