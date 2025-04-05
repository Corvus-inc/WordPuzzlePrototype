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
            if (_levels != null && _currentLevelIndex < _levels.Count - 1)
            {
                _currentLevelIndex++;
                return true;
            }
            Debug.LogWarning("No next level available.");
            return false;
        }

        public void ResetProgress()
        {
            _currentLevelIndex = 0;
        }
    }
}