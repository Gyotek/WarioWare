using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms;


namespace Game.Dinventoryablo
{
    public class DragAndDrop : MonoBehaviour
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
            SavePos();
            colliders = GetComponents<Collider2D>();
        }

        public void SavePos()
        {
            savedPos = transform.position;
        }

        void OnMouseEnter()
        {
            GetComponent<SpriteRenderer>().color = mouseOverColor;
        }

        void OnMouseExit()
        {
            GetComponent<SpriteRenderer>().color = originalColor;
        }

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
            Debug.Log(gameObject.name + " Collide with " + collision.gameObject.name);
            transform.position = savedPos;
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
                transform.position = savedPos;
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
