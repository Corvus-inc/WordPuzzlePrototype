using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LevelService
    {
        private List<LevelData> _levels;
        private int _currentLevelIndex = 0;

        public void SetLevels(List<LevelData> levels)
        {
            _levels = levels;
            _currentLevelIndex = 0;
        }
        
        public LevelData GetCurrentLevel()
        {
            if (_levels == null || _levels.Count == 0)
            {
                Debug.LogError("No levels loaded.");
                return null;
            }
            return _levels[_currentLevelIndex];
        }

        public bool TryAdvanceLevel()
        {
            if (_levels == null || _levels.Count == 0)
            {
                Debug.LogWarning("Level list is empty.");
                return false;
            }

            if (_currentLevelIndex < _levels.Count - 1)
            {
                _currentLevelIndex++;
            }
            else
            {
                // Последний уровень пройден — сбрасываемся на первый
                Debug.Log("Reached last level, restarting from the beginning.");
                _currentLevelIndex = 0;
            }

            return true;
        }


        public void ResetProgress()
        {
            _currentLevelIndex = 0;
        }

        public bool MoveToNextLevel()
        {
            if (_currentLevelIndex + 1 < _levels.Count)
            {
                _currentLevelIndex++;
                return true;
            }

            return false;
        }
    }
}