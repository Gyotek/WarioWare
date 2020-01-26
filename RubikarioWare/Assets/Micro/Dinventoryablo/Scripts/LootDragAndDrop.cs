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
        private Vector2 savedPos;

        private Collider2D[] colliders;

        private bool isStillInPlace = false;

        [SerializeField] VoidEvent OnItemPickedUp = default;
        [SerializeField] VoidEvent OnWin = default;

        [SerializeField] private GameObject Inventory;
        [SerializeField] private Camera myCamera;

        private void Start()
        {
            colliders = GetComponents<Collider2D>();
            SavePos();
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
            GetComponent<SpriteRenderer>().color = mouseOverColor;
            DinventoryabloGameManager.instance.SetCurseur(2);
        }

        void OnMouseExit()
        { 
            GetComponent<SpriteRenderer>().color = originalColor;
            DinventoryabloGameManager.instance.SetCurseur(1);
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

            dragging = true;
            Dinventoryablo.AudioManager.instance.PlaySFX(Dinventoryablo.AudioManager.SFX.Pick);
            DinventoryabloGameManager.instance.SetCurseur(3);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (DinventoryabloGameManager.instance.gameWon)
                return;
            
            isStillInPlace = false;
            ResetPos();
        }

        void OnMouseUp()
        {
            if (DinventoryabloGameManager.instance.gameWon)
            {
                return;
            }
            Debug.Log("Item droped");
            dragging = false;
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
                Dinventoryablo.AudioManager.instance.PlaySFX(Dinventoryablo.AudioManager.SFX.Placed);
                isStillInPlace = true;
                StartCoroutine(IsStillInPlaceCoroutine());
            }
        }

        IEnumerator IsStillInPlaceCoroutine()
        {
            yield return new WaitForSeconds(0.1f);
            if (isStillInPlace)
                OnWin.Raise();
        }

        void Update()
        {
            if (DinventoryabloGameManager.instance.gameEnded)
                return;
            if (dragging)
            {
                Vector3 pos = myCamera.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0;
                transform.position = pos;
            }
        }
    }
}
