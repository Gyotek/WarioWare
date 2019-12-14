using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Dinventoryablo
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private Transform posHautGauche;
        [SerializeField] private Transform posHautDroite;
        [SerializeField] private Transform posBasGauche;
        [SerializeField] private Transform posBasDroite;

        private float inventoryWidth;
        private float inventoryHeight;

        private float squareWidth;
        private float squareHeight;

        private float gauche;
        private float droite;
        private float haut;
        private float bas;

        private void Start()
        {
            inventoryWidth = Vector2.Distance(posHautGauche.position, posHautDroite.position);
            inventoryHeight = Vector2.Distance(posHautGauche.position, posBasGauche.position);

            squareWidth = inventoryWidth / 10;
            squareHeight = inventoryHeight / 6;

            gauche = posHautGauche.position.x;
            droite = posHautDroite.position.x;
            haut = posHautGauche.position.y;
            bas = posBasGauche.position.y;
        }

        public Vector2 WichSquareIsThis(Vector2 pos)
        {
            int ligne = 0;
            int colonne = 0;


            if (pos.x < gauche || pos.x > droite || pos.y > haut || pos.y < bas)
            {
                Debug.Log("Droped outside of the inventory");
                return Vector2.zero;
            }

            for (int i = 1; i <= 10; i++)
            {
                if (pos.x > (gauche + (squareWidth * (i - 1))) && pos.x < (gauche + (squareWidth *i)))
                {
                    colonne = i;
                    i = 11;
                }
            }
            for (int i = 1; i <= 6; i++)
            {
                if (pos.y < (haut - (squareHeight * (i - 1))) && pos.y > (haut - (squareHeight * i)))
                {
                    ligne = i;
                    i = 7;
                }
            }

            Debug.Log("Colonne : " + colonne + " Ligne : " + ligne);

            Vector2 newPos = new Vector2(gauche + (squareWidth * (colonne - 1)) + (squareWidth / 2), haut - (squareHeight * (ligne - 1)) - (squareHeight / 2));

            return (newPos);
        }
    }
}
