using UnityAtoms;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class MouseEvents : MonoBehaviour
    {
        public UnityEvent onMouseDown = default;
        public UnityEvent onMouseEnter = default;
        public BoolUnityEvent onMouseOver = default;
        public UnityEvent onMouseExit = default;
        public UnityEvent onMouseUp = default;
        public BoolUnityEvent onInteractable = default;

        bool interactable = true;
        bool over = false;
        bool rawOver = false;

        private void Start()
        {
            SetInteractable(true);
        }

        public void SetInteractable(bool value)
        {
            interactable = value;
            onInteractable?.Invoke(interactable);
        }

        private void SetMouseOver(bool value)
        {
            over = value;
            onMouseOver?.Invoke(over);
        }

        public void OnMouseDown()
        {
            if (!interactable)
                return;
            onMouseDown?.Invoke();
        }

        public void OnMouseOver()
        {
        }

        public void OnMouseEnter()
        {
            if (!interactable)
                return;
            onMouseEnter?.Invoke();
            SetMouseOver(true);
        }

        public void OnMouseExit()
        {
            onMouseExit?.Invoke();
            SetMouseOver(false);
        }

        public void OnMouseUp()
        {
            if (!interactable)
                return;
            onMouseUp?.Invoke();
        }
    }
}