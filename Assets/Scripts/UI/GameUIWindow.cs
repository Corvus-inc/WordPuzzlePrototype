using System.Collections.Generic;
using System.Linq;
using Core;
using Cysharp.Threading.Tasks;
using Game;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class GameUIWindow : MonoBehaviour
    {
        [Header("Containers")]
        [SerializeField] private Transform _cellsContainer; 
        [SerializeField] private Transform _clustersContainer; 

        [Header("Prefabs")]
        [SerializeField] private GameObject _cellPrefab;  
        [SerializeField] private GameObject _rowPrefab;  
        [SerializeField] private GameObject _clusterPrefab;
        
        [Header("Anything")]
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private Button _winButton;


        private GameFlowController _gameFlowController;
        private UIScreenManager _uiScreenManager;
        private LevelData _level;

        [Inject]
        public void Construct(GameFlowController gameFlowController,
            LevelService levelService,
            UIScreenManager uiScreenManager)
        {
            _gameFlowController = gameFlowController;
            _uiScreenManager = uiScreenManager;

            _level = levelService.GetCurrentLevel();
        }

        private void Start()
        {
            ClearContainer(_cellsContainer);
            ClearContainer(_clustersContainer);

            if (_level != null)
            {
                CreateRows();
                CreateClusterItems(_level.clusters);
                
                _uiScreenManager.SetupBackground(_level.id);
            }

            _winButton.onClick.AddListener(OnWinClicked);
        }

        private void ClearContainer(Transform container)
        {
            for (int i = container.childCount - 1; i >= 0; i--)
            {
                Destroy(container.GetChild(i).gameObject);
            }
        }

        private void CreateRows()
        {
            foreach (var word in _level.words)
            {
                var row = Instantiate(_rowPrefab, _cellsContainer);
                row.SetActive(true);
            }
        }

        private void CreateClusterItems(List<string> clusters)
        {
            clusters = clusters.OrderBy(x => Random.value).ToList();

            foreach (var cluster in clusters)
            {
                GameObject clusterGO = Instantiate(_clusterPrefab, _clustersContainer);
                clusterGO.SetActive(true);

                var clusterScaler = clusterGO.GetComponentInChildren<Cluster>();
                if (clusterScaler != null)
                {
                    clusterScaler.InitCluster(
                        cluster.ToCharArray(),
                        _clustersContainer.parent as RectTransform,
                        _scrollRect);
                }
            }
        }

        private void OnWinClicked()
        {
            Debug.Log("=== OnWinClicked (build words per row) ===");

            var rowPanels = _cellsContainer.GetComponentsInChildren<RowPanel>();
            var allClusters = _clustersContainer.parent.GetComponentsInChildren<Cluster>();

            List<string> constructedWords = new List<string>();

            for (int rowIndex = 0; rowIndex < rowPanels.Length; rowIndex++)
            {
                var row = rowPanels[rowIndex];
                var rowCells = row.GetComponentsInChildren<Cell>();

                HashSet<Cluster> clustersInRow = new HashSet<Cluster>();

                foreach (var cell in rowCells)
                {
                    RectTransform cellRect = cell.GetComponent<RectTransform>();
                    Vector2 cellCenterScreen = RectTransformUtility.WorldToScreenPoint(null, cellRect.position);

                    foreach (var cluster in allClusters)
                    {
                        var clusterRect = cluster.GetComponent<RectTransform>();
                        if (clusterRect == null) continue;

                        if (RectTransformUtility.RectangleContainsScreenPoint(clusterRect, cellCenterScreen, null))
                        {
                            clustersInRow.Add(cluster);
                        }
                    }
                }

                if (clustersInRow.Count > 0)
                {
                    var clusterValues = clustersInRow
                        .Select(c => c.GetValue())
                        .ToList();

                    string word = string.Concat(clusterValues);
                    constructedWords.Add(word);

                    Debug.Log($"Row {rowIndex + 1}: word = {word}");
                }
                else
                {
                    constructedWords.Add("");
                    Debug.Log($"Row {rowIndex + 1}: no clusters");
                }
            }

            Debug.Log("Constructed words list:");
            foreach (var word in constructedWords)
            {
                Debug.Log($"→ {word}");
            }

            if (CheckWin(constructedWords, _level.words))
            {
                _gameFlowController.ShowVictory(constructedWords);
            }else
            {
                FlashButtonRed(_winButton, 3);
            }
        }
        
        private static bool CheckWin(List<string> constructedWords, List<string> targetWords)
        {
            if (constructedWords == null || targetWords == null)
                return false;

            if (constructedWords.Count != targetWords.Count)
                return false;

            var targetSet = new HashSet<string>(
                targetWords.Select(w => w.Trim().ToLowerInvariant())
            );

            foreach (var word in constructedWords)
            {
                string normalized = word?.Trim().ToLowerInvariant();
                if (string.IsNullOrEmpty(normalized) || !targetSet.Contains(normalized))
                {
                    return false;
                }
            }

            return true;
        }
        
        private async UniTask FlashButtonRed(Button button, int times)
        {
            var image = button.GetComponent<Image>();
            if (image == null)
                return;

            Color originalColor = image.color;
            Color flashColor = Color.red;

            for (int i = 0; i < times; i++)
            {
                image.color = flashColor;
                await UniTask.Delay(150);
                image.color = originalColor;
                await UniTask.Delay(150);
            }
        }
    }
}