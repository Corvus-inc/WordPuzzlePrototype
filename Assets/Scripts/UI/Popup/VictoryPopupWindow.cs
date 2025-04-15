using System.Collections.Generic;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace UI.Popup
{
    public class VictoryPopupWindow : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Transform wordListContainer;
        [SerializeField] private TMP_Text wordItemPrefab;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button nextLevelButton;

        private GameFlowController _gameFlowController;

        [Inject]
        public void Construct(GameFlowController gameFlowController)
        {
            _gameFlowController = gameFlowController;
        }

        public void SetWords(List<string> words)
        {
            foreach (Transform child in wordListContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (var word in words)
            {
                var wordUI = Instantiate(wordItemPrefab, wordListContainer);
                wordUI.text = word;
            }
        }

        private void Start()
        {
            mainMenuButton.onClick.AddListener(OnMainMenuClicked);
            nextLevelButton.onClick.AddListener(OnNextLevelClicked);
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