using System;
using Cysharp.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace Core
{
    public struct userAttributes
    {
    }

    public struct appAttributes
    {
    }

    public class RemoteConfigManager
    {
        public async UniTask<string> FetchRemoteLevelJsonAsync(string configKey)
        {
            if (!await InitializeUnityServicesAsync())
            {
                Debug.LogWarning("Unity Services failed to initialize.");
                return string.Empty;
            }

            return await FetchConfigWithTimeoutAsync(configKey, TimeSpan.FromSeconds(10));
        }

        private async UniTask<bool> InitializeUnityServicesAsync()
        {
            try
            {
                if (UnityServices.State != ServicesInitializationState.Initialized)
                {
                    await UnityServices.InitializeAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Unity Services initialization error: {ex}");
                return false;
            }
        }

        private async UniTask<string> FetchConfigWithTimeoutAsync(string configKey, TimeSpan timeout)
        {
            var tcs = new UniTaskCompletionSource<string>();

            void Handler(ConfigResponse response)
            {
                RemoteConfigService.Instance.FetchCompleted -= Handler;

                if (response.status == ConfigRequestStatus.Success && response.requestOrigin == ConfigOrigin.Remote)
                {
                    var levelJson = RemoteConfigService.Instance.appConfig.GetJson(configKey);
                    
                    if (levelJson != null && levelJson.Trim() != "{}")
                    {
                        Debug.Log($"Remote config received for key {configKey}.");
                        tcs.TrySetResult(levelJson);
                    }
                    else
                    {
                        Debug.LogWarning($"Remote config key {configKey} returned empty JSON object.");
                        tcs.TrySetResult(string.Empty);
                    }
                    
                    tcs.TrySetResult(levelJson);
                }
                else
                {
                    Debug.LogWarning($"Failed to fetch remote config for key {configKey}. Status: {response.status}");
                    tcs.TrySetResult(string.Empty);
                }
            }

            RemoteConfigService.Instance.FetchCompleted += Handler;

            try
            {
                RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
            }
            catch (Exception ex)
            {
                Debug.LogError($"Remote Config fetch error: {ex}");
                RemoteConfigService.Instance.FetchCompleted -= Handler;
                return string.Empty;
            }

            // Заменяем WithCancellation на WhenAny
            var fetchTask = tcs.Task;
            var timeoutTask = UniTask.Delay(timeout);

            var (hasResultLeft, result) = await UniTask.WhenAny(fetchTask, timeoutTask);

            if (!hasResultLeft)
            {
                RemoteConfigService.Instance.FetchCompleted -= Handler;
                Debug.LogWarning("Remote Config fetch timed out.");
                return string.Empty;
            }

            return await fetchTask;
        }
    }
}