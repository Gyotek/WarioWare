using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms;

namespace Game.ScratchyBird
{
    public class ScratchyController : MonoBehaviour
    {
        private Rigidbody2D rb;
        [SerializeField] private GameObject bloodSprite;
        [SerializeField] private ParticleSystem bloodParticles;
        [SerializeField] private ParticleSystem multicolorBloodParticles;
        [SerializeField] private VoidEvent onImpale;
        [SerializeField] private VoidEvent onImpact;

        public float velocity = 1.4f;
        public float speed;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.simulated = false;
        }

        // Update is called once per frame
        void Update()
        {

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, rb.velocity.y *10), 0.5f);
            /*
            if(transform.rotation.z > 0.4 && rb.velocity.y > 0)
            {
                transform.Rotate(0, 0, rb.velocity.y * 2);
                Debug.Log("up");
            }
            else if (transform.rotation.z < 0.4 && rb.velocity.y < 0)
            {
                transform.Rotate(0, 0, rb.velocity.y * 2);
                Debug.Log("down");
            }*/

            if (rb.simulated)
            {
                Fly();
            }
            //Move();
        }

        public void StartFlying()
        {
            rb.simulated = true;
            if (Macro.BPM == 120)
            {
                speed = speed * 1.75f;
                rb.gravityScale = 1.2f;
                rb.velocity = rb.velocity * 2.5f;
            }
            else if (Macro.BPM == 160)
            {
                speed = speed * 2.5f;
                rb.gravityScale = 1.4f;
                rb.velocity = rb.velocity * 3f;
            }
        }

        private void Move()
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        private void Fly()
        {
            if (Input.GetMouseButtonDown(0))
            {
                rb.velocity = Vector2.up * velocity;
                ScratchyBird.AudioManager.instance.PlaySFX(AudioManager.SFX.WingFlap);
            }
        }
         private void Stop()
        {
            rb.simulated = false;
            velocity = 0;
            speed = 0;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Stop();

            if (collision.gameObject.GetComponent<Spikes>())
            {
                multicolorBloodParticles.Play();
                bloodSprite.SetActive(true);
                ScratchyBird.AudioManager.instance.PlaySFX(AudioManager.SFX.Loose);
                ScratchyBird.AudioManager.instance.PlaySFX(AudioManager.SFX.Win);
                ScratchyBird.AudioManager.instance.PlaySFX(AudioManager.SFX.Applauses);
                onImpale.Raise();
            }
            else if (collision.gameObject.GetComponent<Tube>())
            {
                bloodParticles.Play();
                bloodSprite.SetActive(true);
                ScratchyBird.AudioManager.instance.PlaySFX(AudioManager.SFX.LooseTube);
                onImpact.Raise();
            }
            else if (collision.gameObject.GetComponent<End>())
            {
                onImpact.Raise();
            }
            else
            {
                onImpact.Raise();
                bloodParticles.Play();
                ScratchyBird.AudioManager.instance.PlaySFX(AudioManager.SFX.Loose);
                bloodSprite.SetActive(true);

            }


        }
    }
}
