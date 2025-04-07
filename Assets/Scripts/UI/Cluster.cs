    using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Cluster : MonoBehaviour
    {
        [SerializeField] private RectTransform horizontalGroup;
        [SerializeField] private GameObject clusterCellPrefab;
        [SerializeField] private Image foreground;

        public void InitCluster(char[] content, RectTransform dragParent, ScrollRect scrollRect)
        {
            foreach (var ch in content)
            {
                var clusterCell = Instantiate(clusterCellPrefab, horizontalGroup);
                clusterCell.SetActive(true);

                var text = clusterCell.GetComponentInChildren<TMP_Text>();
                if (text != null)
                {
                    text.text = ch.ToString();
                }
            }

            var layout = GetComponent<LayoutElement>();
            if (layout != null)
            {
                layout.preferredWidth = 110f * content.Length;
            }
            
            if (foreground != null)
            {
                foreground.color = GetRandomSoftColor();
            }
            
            var drag = GetComponent<DraggableCluster>();
            if (drag != null)
            {
                drag.Initialize(dragParent, scrollRect);
            }

            
            Canvas.ForceUpdateCanvases();

            var hLayout = horizontalGroup.GetComponent<HorizontalLayoutGroup>();
            if (hLayout != null)
            {
                hLayout.enabled = false;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(horizontalGroup);
        }
        
        private Color GetRandomSoftColor()
        {
            float h = Random.value;
            float s = Random.Range(0.4f, 0.7f);
            float v = Random.Range(0.8f, 1.0f);
            return Color.HSVToRGB(h, s, v);
        }
    }
}