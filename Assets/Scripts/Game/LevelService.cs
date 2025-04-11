using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Game
{
    public class LevelService
    {
        private List<LevelData> _levels;
        private int _currentLevelIndex;

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

            if (TryMoveToNextLevel()) return true;
            
            Debug.Log("Reached last level, restarting from the beginning.");
            _currentLevelIndex = 0;

            return true;
        }
        
        private bool TryMoveToNextLevel()
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