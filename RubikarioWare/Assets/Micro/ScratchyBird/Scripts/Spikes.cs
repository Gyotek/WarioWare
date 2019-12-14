using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Flying>())
        {
            collision.gameObject.GetComponent<Flying>().velocity = 0;
            collision.gameObject.GetComponent<Move>().speed = 0;
        }
    }
}
