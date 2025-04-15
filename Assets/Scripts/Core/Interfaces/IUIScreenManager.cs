using Cysharp.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUIScreenManager
    {
        void ShowMainMenu();
        void ShowGameUI();
        UniTask SetupBackground(int levelID);
    }
}