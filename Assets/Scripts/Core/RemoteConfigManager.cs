using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace Core
{
    public struct userAttributes { }
    public struct appAttributes { }
    
    public class RemoteConfigManager
    {
        private static readonly TimeSpan FETCH_TIMEOUT = TimeSpan.FromSeconds(10);

        private static readonly string[] RemoteLevelKeys = { "level_1", "level_2", "level_3", "level_4" };
        
        public async UniTask<string> FetchRemoteLevelJsonAsync(string configKey)
        {
            bool prepared = await PrepareRemoteConfig();
            if (!prepared)
            {
                Debug.LogWarning("Unable to fetch remote config.");
                return string.Empty;
            }
            return GetJsonForKey(configKey);
        }

        public async UniTask<List<string>> FetchAllLevelsAsync()
        {
            var result = new List<string>();

            bool prepared = await PrepareRemoteConfig();
            if (!prepared)
            {
                Debug.LogWarning("Unable to fetch remote config for multiple keys.");
                return result;
            }

            foreach (string key in RemoteLevelKeys)
            {
                string json = GetJsonForKey(key);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    result.Add(json);
                    Debug.Log($"Remote config for '{key}': OK");
                }
                else
                {
                    Debug.LogWarning($"Remote config for '{key}' is empty or missing.");
                }
            }

            return result;
        }
        
        private async UniTask<bool> PrepareRemoteConfig()
        {
            if (!await InitializeUnityServicesAsync())
                return false;

            return await WaitForFetchConfigsAsync();
        }

        private string GetJsonForKey(string configKey)
        {
            string json = RemoteConfigService.Instance.appConfig.GetJson(configKey);
            if (!string.IsNullOrWhiteSpace(json) && json.Trim() != "{}")
                return json;
            return string.Empty;
        }

        private async UniTask<bool> InitializeUnityServicesAsync()
        {
            try
            {
                if (UnityServices.State != ServicesInitializationState.Initialized)
                    await UnityServices.InitializeAsync();
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Unity Services initialization error: {ex}");
                return false;
            }
        }

        private async UniTask<bool> WaitForFetchConfigsAsync()
        {
            var tcs = new UniTaskCompletionSource<bool>();

            RemoteConfigService.Instance.FetchCompleted += Handler;

            try
            {
                RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
            }
            catch (Exception ex)
            {
                Debug.LogError($"Remote Config fetch error: {ex}");
                RemoteConfigService.Instance.FetchCompleted -= Handler;
                return false;
            }

            var fetchTask = tcs.Task;
            var timeoutTask = UniTask.Delay(FETCH_TIMEOUT);

            var (hasResultLeft, _) = await UniTask.WhenAny(fetchTask, timeoutTask);

            if (!hasResultLeft)
            {
                RemoteConfigService.Instance.FetchCompleted -= Handler;
                Debug.LogWarning("Remote Config fetch timed out.");
                return false;
            }

            return await fetchTask;

            void Handler(ConfigResponse response)
            {
                RemoteConfigService.Instance.FetchCompleted -= Handler;
                if (response.status == ConfigRequestStatus.Success && response.requestOrigin == ConfigOrigin.Remote)
                {
                    Debug.Log("Remote Config fetch successful.");
                    tcs.TrySetResult(true);
                }
                else
                {
                    Debug.LogWarning($"Remote Config fetch failed. Status: {response.status}");
                    tcs.TrySetResult(false);
                }
            }
        }
    }
}
