using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms;

namespace Game.ScratchyBird
{
    public class ScratchyController : MonoBehaviour
    {
        private Rigidbody2D rb;

        [SerializeField] private VoidEvent onImpale;
        [SerializeField] private VoidEvent onImpact;

        public float velocity = 1.4f;
        public float speed;

        // Start is called before the first frame update
        void Start()
        {
            
            rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            Fly();
            //Move();
        }

        private void Move()
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        private void Fly()
        {
            if (Input.GetMouseButtonDown(0))
                rb.velocity = Vector2.up * velocity;
        }
         private void Stop()
        {
            velocity = 0;
            speed = 0;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Stop();

            if (collision.gameObject.GetComponent<Spikes>())
                onImpale.Raise();
            else
                onImpact.Raise();

        }
    }
}
