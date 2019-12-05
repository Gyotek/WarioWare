using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{

    private Color mouseOverColor = Color.gray;
    private Color originalColor = Color.white;
    private bool dragging = false;
    private float distance;
    [SerializeField] private Transform parentObjectTr;

    void OnMouseEnter()
    {
        Debug.Log("MouseEnter : " + this.gameObject.name);
        GetComponent<SpriteRenderer>().color = mouseOverColor;
    }

    void OnMouseExit()
    {
        Debug.Log("MouseExit : " + this.gameObject.name);
        GetComponent<SpriteRenderer>().color = originalColor;
    }

    void OnMouseDown()
    {
        Debug.Log("MouseDown");
        //distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        dragging = true;
    }

    void OnMouseUp()
    {
        Debug.Log("MouseUp");
        dragging = false;
    }

    void Update()
    {
        if (dragging)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            parentObjectTr.position = pos;
        }
    }
}
