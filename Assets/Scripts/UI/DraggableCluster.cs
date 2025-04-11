using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class DraggableCluster : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private RectTransform _dragParent;
        private ScrollRect _scrollRect;

        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private bool _isOutside;
        private Vector2 _offset;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        }

        public void Initialize(RectTransform dragParent, ScrollRect scrollRect)
        {
            _dragParent = dragParent;
            _scrollRect = scrollRect;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _scrollRect.OnBeginDrag(eventData);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, eventData.position, eventData.pressEventCamera, out _offset);

            _canvasGroup.blocksRaycasts = false;
            _isOutside = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isOutside)
            {
                _scrollRect.OnDrag(eventData);

                if (!IsPointerInsideScrollRect(eventData))
                {
                    _isOutside = true;
                    
                    _rectTransform.SetParent(_dragParent, true);
                    _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                }
            }
            else
            {
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_dragParent, eventData.position, eventData.pressEventCamera, out Vector2 localPos))
                {
                    _rectTransform.anchoredPosition = localPos - _offset;
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _scrollRect.OnEndDrag(eventData);

            _canvasGroup.blocksRaycasts = true;
        }

        private bool IsPointerInsideScrollRect(PointerEventData eventData)
        {
            var srRect = _scrollRect.viewport != null ? _scrollRect.viewport : _scrollRect.GetComponent<RectTransform>();
            return RectTransformUtility.RectangleContainsScreenPoint(srRect, eventData.position, eventData.pressEventCamera);
        }
    }
}
