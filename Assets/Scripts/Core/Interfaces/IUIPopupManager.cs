using System.Collections.Generic;

namespace Core.Interfaces
{
    public interface IUIPopupManager
    {
        void ShowVictoryPopup(List<string> solvedWords);
        void ShowSettingsPopup();
    }
}