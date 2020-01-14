using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class Droppable : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] LayoutGroup layoutGroup = default;

        bool hover = false;

        public void OnDrop(PointerEventData eventData)
        {
            layoutGroup.SetLayoutHorizontal();
            layoutGroup.SetLayoutVertical();
            if (!hover)
                return;
            if (eventData.pointerDrag == null)
                return;
            RectTransform rect = eventData.pointerDrag.transform as RectTransform;
            rect.SetParent(transform as RectTransform);
            transform.SetAsFirstSibling();
        }

        public void OnPointerExit(PointerEventData eventData) => hover = false;

        public void OnPointerEnter(PointerEventData eventData) => hover = true;
    }
}