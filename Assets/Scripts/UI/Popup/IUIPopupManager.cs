using System.Collections.Generic;

namespace UI.Popup
{
    public interface IUIPopupManager
    {
        void ShowVictoryPopup(List<string> solvedWords);
        void ShowSettingsPopup();
    }
}