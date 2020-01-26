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
            colliders = GetComponents<Collider2D>();
            SavePos();
        }

        public void InitPos()
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = false;
            }

            transform.position = Inventory.GetComponent<InventoryManager>().WichSquareIsThis(transform.position);

            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = true;
            }

        }

        public void SavePos() =>
            savedPos = transform.position;

        public void ResetPos()
        {
            Dinventoryablo.AudioManager.instance.PlaySFX(Dinventoryablo.AudioManager.SFX.Nop);
            transform.position = savedPos;
        }
            

        void OnMouseEnter()
        {
            DinventoryabloGameManager.instance.SetCurseur(2);
            GetComponent<SpriteRenderer>().color = mouseOverColor;
        }

        void OnMouseExit()
        {
            DinventoryabloGameManager.instance.SetCurseur(1);
            GetComponent<SpriteRenderer>().color = originalColor;
        }



        void OnMouseDown()
        {
            if (DinventoryabloGameManager.instance.gameEnded)
            {
                return;
            }
            Debug.Log("Item picked");
            OnItemPickedUp.Raise();
            for (int i = 0; i < colliders.Length; i ++)
            {
                colliders[i].enabled = false;
            }
            SavePos();
            dragging = true;
            Dinventoryablo.AudioManager.instance.PlaySFX(Dinventoryablo.AudioManager.SFX.Pick);
            DinventoryabloGameManager.instance.SetCurseur(3);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            Debug.Log(gameObject.name + " Collide with " + collision.gameObject.name);
            ResetPos();
        }

        void OnMouseUp()
        {
            if (DinventoryabloGameManager.instance.gameEnded)
            {
                return;
            }
            Debug.Log("Item droped");
            dragging = false;
            Dinventoryablo.AudioManager.instance.PlaySFX(Dinventoryablo.AudioManager.SFX.Placed);
            DinventoryabloGameManager.instance.SetCurseur(1);

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
            if (DinventoryabloGameManager.instance.gameEnded)
            {
                if (dragging)
                    ResetPos();
                return;
            }
            if (dragging)
            {
                Vector3 pos = myCamera.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                transform.position = pos;
            }
        }
    }
}
