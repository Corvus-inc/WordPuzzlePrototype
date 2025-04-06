namespace Game
{
    public class GameFlowController
    {
        private readonly UIManager _uiManager;

        private SceneState _currentState;

        public GameFlowController(
            UIManager uiManager)
        {
            _uiManager = uiManager;
            _currentState = SceneState.Menu;
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
            throw new System.NotImplementedException();
        }
    }

}