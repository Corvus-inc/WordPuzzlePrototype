using Game;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Popup
{
    public class SettingsPopupWindow : MonoBehaviour
    {
        [SerializeField] private Toggle musicToggle;
        [SerializeField] private Button closeButton;
        
        private IMusicManager _musicManager;
        
        [Inject]
        public void Construct(IMusicManager musicManager)
        {
            _musicManager = musicManager;
            
            musicToggle.isOn = musicManager.IsMusicEnabled;
            musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
            closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        private void OnCloseButtonClicked()
        {
            Destroy(gameObject);
        }

        private void OnMusicToggleChanged(bool arg0)
        {
            if (arg0)
            {
                _musicManager.PlayMusic();
            }
            else
            {
                _musicManager.StopMusic();
            }
        }
    }
}