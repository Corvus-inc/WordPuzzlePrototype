using System.Collections.Generic;
using Game.Signals;
using UnityEngine;
using Zenject;

namespace Game
{
    public class GameFlowController
    {
        private readonly SignalBus _signalBus;
        private readonly LevelService _levelService;

        public GameFlowController(SignalBus signalBus, LevelService levelService)
        {
            _signalBus = signalBus;
            _levelService = levelService;
        }

        public void GoToMenu()
        {
            _signalBus.Fire<ShowMainMenuSignal>();
        }

        public void StartGame()
        {
            _signalBus.Fire<ShowGameUISignal>();
        }

        public void ShowVictory(List<string> solvedWords)
        {
            _signalBus.Fire(new ShowVictorySignal { Words = solvedWords });
        }

        public void LoadNextLevel()
        {
            bool hasNext = _levelService.TryAdvanceLevel();
            if (hasNext)
            {
                _signalBus.Fire<ShowGameUISignal>();
            }
            else
            {
                Debug.Log("No more levels!");
                _signalBus.Fire<ShowMainMenuSignal>();
            }
        }
    }
}