using Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class LevelService
    {
        private readonly RemoteConfigManager _remoteConfigManager;

        public LevelService(RemoteConfigManager remoteConfigManager)
        {
            _remoteConfigManager = remoteConfigManager;
        }

        public async UniTask<LevelData> LoadLevelAsync(string levelKey)
        {
            string levelJson = string.Empty;
            string remoteLevelJson = await _remoteConfigManager.FetchRemoteLevelJsonAsync(levelKey);

            if (!string.IsNullOrEmpty(remoteLevelJson))
            {
                Debug.Log($"Level {levelKey} loaded from Remote Config.");
                levelJson = remoteLevelJson;
            }
            else
            {
                TextAsset levelAsset = Resources.Load<TextAsset>($"Configs/{CoreConstants.DefaultLevelKey}");
                if (levelAsset == null)
                {
                    Debug.LogError($"Default level file not found for key: {CoreConstants.DefaultLevelKey}");
                    return null;
                }

                Debug.Log($"Level {CoreConstants.DefaultLevelKey} loaded from default Resources.");
                levelJson = levelAsset.text;
            }

            LevelData levelData = JsonUtility.FromJson<LevelData>(levelJson);
            if (levelData == null)
            {
                Debug.LogError($"Failed to deserialize level data for level {levelKey}.");
                return null;
            }

            return levelData;
        }
    }
}