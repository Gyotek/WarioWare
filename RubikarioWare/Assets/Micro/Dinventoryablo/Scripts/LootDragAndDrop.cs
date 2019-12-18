using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms;


namespace Game.Dinventoryablo
{
    public class LootDragAndDrop : MonoBehaviour
    {

        private Color mouseOverColor = Color.gray;
        private Color originalColor = Color.white;
        private bool dragging = false;
        private float distance;
        private Collider2D[] colliders;
        private Vector2 savedPos;

        [SerializeField] VoidEvent OnItemPickedUp = default;
        [SerializeField] private GameObject Inventory;
        [SerializeField] private Camera myCamera;

        private void Start()
        {
            colliders = GetComponents<Collider2D>();

            SavePos();
        }

        public void SavePos() =>
            savedPos = transform.position;

        public void ResetPos() =>
            transform.position = savedPos;

        void OnMouseEnter() =>
            GetComponent<SpriteRenderer>().color = mouseOverColor;

        void OnMouseExit() =>
            GetComponent<SpriteRenderer>().color = originalColor;

        void OnMouseDown()
        {
            Debug.Log("Item picked");
            OnItemPickedUp.Raise();
            for (int i = 0; i < colliders.Length; i ++)
            {
                colliders[i].enabled = false;
            }

            dragging = true;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            ResetPos();
        }

        void OnMouseUp()
        {
            Debug.Log("Item droped");
            dragging = false;

            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = true;
            }

            if (Inventory.GetComponent<InventoryManager>().WichSquareIsThis(transform.position) == Vector2.zero)
            {
                ResetPos();
            }
            else
            {
                transform.position = Inventory.GetComponent<InventoryManager>().WichSquareIsThis(transform.position);
            }
        }
        void Update()
        {
            if (dragging)
            {
                Vector3 pos = myCamera.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                transform.position = pos;
            }
        }
    }
}
