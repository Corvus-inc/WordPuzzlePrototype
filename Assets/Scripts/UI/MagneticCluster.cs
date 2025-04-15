using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class MagneticCluster : MonoBehaviour, IEndDragHandler
    {
        [SerializeField] private Transform clusterCellsParent;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var rowPanels = FindObjectsByType<RowPanel>(FindObjectsSortMode.None);
            MagneticCluster currentCluster = null;
            List<Cell> clusterCells = new List<Cell>();

            foreach (var row in rowPanels)
            {
                var rowCells = row.GetComponentsInChildren<Cell>();

                foreach (var cell in rowCells)
                {
                    RectTransform cellRect = cell.GetComponent<RectTransform>();
                    Vector2 cellCenterScreen = RectTransformUtility.WorldToScreenPoint(null, cellRect.position);

                    if (RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, cellCenterScreen, null))
                    {
                        if (currentCluster != this)
                        {
                            if (currentCluster != null)
                                CenterClusterOnLastCell(currentCluster, clusterCells);

                            currentCluster = this;
                            clusterCells.Clear();
                        }

                        clusterCells.Add(cell);
                    }
                }

                if (currentCluster != null && clusterCells.Count > 0)
                {
                    CenterClusterOnLastCell(currentCluster, clusterCells);
                    currentCluster = null;
                    clusterCells.Clear();
                }
            }
        }

        private void CenterClusterOnLastCell(MagneticCluster cluster, List<Cell> clusterCells)
        {
            if (clusterCells == null || clusterCells.Count == 0) return;

            var lastCell = clusterCells[^1];
            var cellRect = lastCell.GetComponent<RectTransform>();
            var cellsInCluster = Enumerable.Range(0, clusterCellsParent.childCount)
                .Select(i => clusterCellsParent.GetChild(i).GetComponent<RectTransform>())
                .Where(r => r != null)
                .ToArray();

            var lastCellInCluster = cellsInCluster[^1];
            var clusterRect = cluster.GetComponent<RectTransform>();

            Vector2 targetCellScreenCenter = RectTransformUtility.WorldToScreenPoint(null, cellRect.position);

            Vector3 localOffset = lastCellInCluster.localPosition;
            Vector2 pivotOffset = new Vector2(
                lastCellInCluster.rect.width * (0.5f - lastCellInCluster.pivot.x),
                lastCellInCluster.rect.height * (0.5f - lastCellInCluster.pivot.y)
            );

            localOffset += (Vector3)pivotOffset;

            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                clusterRect.parent as RectTransform,
                targetCellScreenCenter,
                null,
                out var targetWorldCenter
            );

            clusterRect.position = targetWorldCenter - clusterRect.TransformVector(localOffset);

            Debug.Log($"Cluster '{cluster.name}' aligned by its inner cell '{lastCellInCluster.name}' with field cell '{lastCell.name}'");
        }
    }
}