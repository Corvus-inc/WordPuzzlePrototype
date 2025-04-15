using Cysharp.Threading.Tasks;

namespace UI.Screen
{
    public interface IUIScreenManager
    {
        void ShowMainMenu();
        void ShowGameUI();
        UniTask SetupBackground(int levelID);
    }
}