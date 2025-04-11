using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameFlowController
    {
        private readonly UIScreenManager _uiScreenManager;
        private readonly PopupController _popupController;
        private readonly LevelService _levelService;

        private SceneState _currentState;

        public GameFlowController(
            UIScreenManager uiScreenManager,
            PopupController popupController,
            LevelService levelService)
        {
            _uiScreenManager = uiScreenManager;
            _currentState = SceneState.Menu;
            _popupController = popupController;
            _levelService = levelService;
        }

        public void GoToMenu()
        {
            _currentState = SceneState.Menu;
            _uiScreenManager.ShowMainMenu();
        }
        
        public void StartGame()
        {
            _currentState = SceneState.Game;
            _uiScreenManager.ShowGameUI();
        }

        public void ShowVictory(List<string> solvedWords)
        {
            _popupController.ShowVictoryPopup(solvedWords);
        }

        public void LoadNextLevel()
        {
            bool hasNext = _levelService.TryAdvanceLevel();
            
            if (hasNext)
            {
                _uiScreenManager.ShowGameUI();
            }
            else
            {
                Debug.Log("No more levels!");
                GoToMenu();
            }
        }
    }

}