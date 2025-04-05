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
        }

        public void GoToMenu()
        {
            _currentState = SceneState.Menu;
            _uiManager.ShowMainMenu();
        }
    }

}