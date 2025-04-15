using System.Collections.Generic;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Popup
{
    public class VictoryPopupWindow : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Transform _wordListContainer;
        [SerializeField] private TMP_Text _wordItemPrefab;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _nextLevelButton;

        private GameFlowController _gameFlowController;

        [Inject]
        public void Construct(GameFlowController gameFlowController)
        {
            _gameFlowController = gameFlowController;
        }

        public void SetWords(List<string> words)
        {
            foreach (Transform child in _wordListContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (var word in words)
            {
                var wordUI = Instantiate(_wordItemPrefab, _wordListContainer);
                wordUI.text = word;
            }
        }

        private void Start()
        {
            _mainMenuButton.onClick.AddListener(OnMainMenuClicked);
            _nextLevelButton.onClick.AddListener(OnNextLevelClicked);
        }

        private void OnMainMenuClicked()
        {
            _gameFlowController.GoToMenu();
            Destroy(gameObject);
        }

        private void OnNextLevelClicked()
        {
            _gameFlowController.LoadNextLevel();
            Destroy(gameObject);
        }
    }
}