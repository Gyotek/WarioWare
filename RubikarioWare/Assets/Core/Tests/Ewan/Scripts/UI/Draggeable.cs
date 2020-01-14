using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class Draggeable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public void OnBeginDrag(PointerEventData eventData)
        {
            eventData.pointerDrag = gameObject;
            SetDraggedPosition(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            SetDraggedPosition(eventData);
        }

        private void SetDraggedPosition(PointerEventData eventData)
        {
            var rectTrasform = transform as RectTransform;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                rectTrasform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector3 globalMousePos))
            {
                rectTrasform.position = globalMousePos;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            eventData.pointerDrag = null;
        }
    }
}