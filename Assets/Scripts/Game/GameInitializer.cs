using System;
using System.Collections.Generic;
using UniRx;
using Zenject;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game
{
    public class GameInitializer : IInitializable, IDisposable
    {
        private readonly LevelListProvider _levelListProvider;
        private readonly LevelService _levelService;

        private readonly CompositeDisposable _disposables = new ();

        public GameInitializer(
            LevelListProvider levelListProvider,
            LevelService levelService)
        {
            _levelListProvider = levelListProvider;
            _levelService = levelService;
        }

        public void Initialize()
        {
            _levelListProvider.LoadInitialLevel();
            
            _levelListProvider.Levels
                .Subscribe(OnLevelsUpdated)
                .AddTo(_disposables); 
            
            FetchRemoteLevels().Forget();
        }

        private async UniTaskVoid FetchRemoteLevels()
        {
            await _levelListProvider.TryFetchRemoteLevelsAsync();
        }

        private void OnLevelsUpdated(List<LevelData> newLevels)
        {
            if (newLevels == null || newLevels.Count == 0)
            {
                Debug.LogWarning("No levels available (empty).");
                return;
            }

            _levelService.SetLevels(newLevels);

            var currentLevel = _levelService.GetCurrentLevel();
            if (currentLevel != null)
            {
                Debug.Log($"GameInitializer: Current level ID = {currentLevel.id}");
            }
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
