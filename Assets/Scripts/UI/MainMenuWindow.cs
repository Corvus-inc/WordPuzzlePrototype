using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Game;

public class MainMenuWindow : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;

    private LevelService _levelService;
    private GameFlowController _gameFlowController;

    [Inject]
    public void Construct(LevelService levelService, GameFlowController gameFlowController)
    {
        _levelService = levelService;
        _gameFlowController = gameFlowController;
    }

    private void Start()
    {
        _playButton.onClick.AddListener(OnPlayClicked);
        _settingsButton.onClick.AddListener(OnSettingsClicked);
    }

    private void OnPlayClicked()
    {
        var level = _levelService.GetCurrentLevel();
        Debug.Log($"[MainMenu] Play clicked. Current level ID: {level?.id}");
        _gameFlowController.StartGame();
    }

    private void OnSettingsClicked()
    {
        Debug.Log("[MainMenu] Settings clicked");
    }
}