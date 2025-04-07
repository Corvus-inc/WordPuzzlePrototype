using Game;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class VictoryPopupWindow : MonoBehaviour
{
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _closeButton;

    private GameFlowController _gameFlowController;

    [Inject]
    public void Construct(GameFlowController gameFlowController)
    {
        _gameFlowController = gameFlowController;
    }

    private void Start()
    {
        _nextButton.onClick.AddListener(OnNextClicked);
        _closeButton?.onClick.AddListener(Close);
    }

    private void OnNextClicked()
    {
        _gameFlowController.LoadNextLevel();
        Close();
    }

    private void Close()
    {
        Destroy(gameObject);
    }
}