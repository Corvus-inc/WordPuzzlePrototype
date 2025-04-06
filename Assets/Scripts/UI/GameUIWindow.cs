using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game
{
    public class GameUIWindow : MonoBehaviour
    {
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private TMP_Text _targetText;
        [SerializeField] private Button _winButton;

        private LevelData _currentLevel;
        private GameFlowController _gameFlowController;

        [Inject]
        public void Construct(GameFlowController gameFlowController, LevelService levelService)
        {
            _gameFlowController = gameFlowController;
            
            _currentLevel = levelService.GetCurrentLevel();
            if (_currentLevel == null)
            {
                Debug.LogWarning("No level data provided to GameUIWindow.");
                return;
            }
            SetLevel(_currentLevel);
        }

        private void Start()
        {
            _winButton.onClick.AddListener(OnWinClicked);
        }

        private void SetLevel(LevelData level)
        {
            _currentLevel = level;

            _titleText.text = $"Level {level.id}";
            _descriptionText.text = level.words.ToString();
            _targetText.text = $"Target: {level.clusters}";
        }

        private void OnWinClicked()
        {
            _gameFlowController.ShowVictory();
        }
    }
}