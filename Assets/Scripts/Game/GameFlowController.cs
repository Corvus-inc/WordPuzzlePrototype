using System.Collections.Generic;
using Core;
using Core.Interfaces;
using UnityEngine;

namespace Game
{
    public class GameFlowController
    {
        private readonly IUIScreenManager _uiScreenManager;
        private readonly IUIPopupManager _uiPopupManager;
        private readonly LevelService _levelService;

        public GameFlowController(
            IUIScreenManager uiScreenManager,
            IUIPopupManager uiPopupManager,
            LevelService levelService)
        {
            _uiScreenManager = uiScreenManager;
            _uiPopupManager = uiPopupManager;
            _levelService = levelService;
        }

        public void GoToMenu()
        {
            _uiScreenManager.ShowMainMenu();
        }
        
        public void StartGame()
        {
            _uiScreenManager.ShowGameUI();
        }

        public void ShowVictory(List<string> solvedWords)
        {
            _uiPopupManager.ShowVictoryPopup(solvedWords);
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