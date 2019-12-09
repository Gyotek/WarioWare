using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        inventoryWidth = Vector2.Distance(posHautGauche.position, posHautDroite.position);
        inventoryHeight = Vector2.Distance(posBasGauche.position, posBasDroite.position);

        squareWidth = inventoryWidth / 10;
        squareHeight = inventoryHeight / 6;
    }

    private int wichSquareIsThis(Vector2 pos)
    {
        if (pos.x < posHautGauche.position.x || pos.x > posHautDroite.position.x || pos.y < posHautGauche.position.y || pos.y > posBasGauche.position.y)
        {
            Debug.Log("Droped outside of the inventory");
            return 0;
        }


        return 0;
    }
}
