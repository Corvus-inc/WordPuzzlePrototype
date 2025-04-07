using System.Collections.Generic;
using System.Linq;
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
        private UIManager _uiManager;
        private LevelData _level;

        [Inject]
        public void Construct(GameFlowController gameFlowController,
            LevelService levelService,
            UIManager uiManager)
        {
            _gameFlowController = gameFlowController;
            _uiManager = uiManager;

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
                
                var back = Resources.Load<Sprite>($"UI/Background/Back{_level.id}");
                _uiManager.SetupBackground(back);
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
            _gameFlowController.ShowVictory(_level.words);
        }
    }
}