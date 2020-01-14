using UnityEngine;

namespace Game
{
    public class MouseEventsLinker : MonoBehaviour
    {
        [SerializeField] MouseEvents mouseEvents;

        private void OnValidate()
        {
            if (mouseEvents != null)
                return;
            mouseEvents = GetComponentInParent<MouseEvents>();
        }

        private void OnMouseDown() => mouseEvents.OnMouseDown();

        private void OnMouseEnter() => mouseEvents.OnMouseEnter();

        private void OnMouseOver() => mouseEvents.OnMouseOver();

        private void OnMouseExit() => mouseEvents.OnMouseExit();

        private void OnMouseUp() => mouseEvents.OnMouseUp();
    }

}