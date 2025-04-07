namespace Game
{
    public class GameFlowController
    {
        private readonly UIManager _uiManager;
        private readonly PopupController _popupController;

        private SceneState _currentState;

        public GameFlowController(
            UIManager uiManager,
            PopupController popupController)
        {
            _uiManager = uiManager;
            _currentState = SceneState.Menu;
            _popupController = popupController;
        }

        public void GoToMenu()
        {
            _currentState = SceneState.Menu;
            _uiManager.ShowMainMenu();
        }
        
        public void StartGame()
        {
            _currentState = SceneState.Game;
            _uiManager.ShowGameUI();
        }

        public void ShowVictory()
        {
            _popupController.ShowVictoryPopup();
        }
    }

}