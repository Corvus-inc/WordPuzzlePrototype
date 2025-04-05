using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Game
{
    public class LevelListProvider
    {
        public ReactiveProperty<List<LevelData>> Levels { get; } = new();
        
        private readonly RemoteConfigManager _remoteConfigManager;
        private readonly string _defaultLevelPath = $"Configs/{CoreConstants.DefaultLevelKey}";
        
        public LevelListProvider(RemoteConfigManager remoteConfigManager)
        {
            _remoteConfigManager = remoteConfigManager;
        }

        public void LoadInitialLevel()
        {
            TextAsset levelAsset = Resources.Load<TextAsset>(_defaultLevelPath);
            if (levelAsset == null)
            {
                Debug.LogError($"Default level not found at {_defaultLevelPath}");
                return;
            }

            LevelData defaultLevel = JsonUtility.FromJson<LevelData>(levelAsset.text);
            Levels.Value = new List<LevelData> { defaultLevel };
        }

        public async UniTask TryFetchRemoteLevelsAsync()
        {
            List<string> remoteJson = await _remoteConfigManager.FetchAllLevelsAsync();

            if (remoteJson == null)
            {
                Debug.LogWarning("Remote levels not found, continuing with default.");
                return;
            }

            try
            {
                Levels.Value = ParseLevelsBuId(remoteJson);
                
                Debug.Log($"Remote levels loaded: {Levels.Value.Count} levels.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to parse remote level list: {ex}");
            }
        }

        private static List<LevelData> ParseLevelsBuId(IEnumerable<string> allJsonStrings)
        {
            var levels = new List<LevelData>();

            foreach (var jsonStr in allJsonStrings)
            {
                if (string.IsNullOrWhiteSpace(jsonStr))
                    continue;

                LevelData data = JsonUtility.FromJson<LevelData>(jsonStr);
                if (data != null)
                {
                    levels.Add(data);
                }
                else
                {
                    Debug.LogWarning($"Failed to parse LevelData from JSON: {jsonStr}");
                }
            }

            levels = levels.OrderBy(l => l.id).ToList();

            return levels;
        }
    }
}