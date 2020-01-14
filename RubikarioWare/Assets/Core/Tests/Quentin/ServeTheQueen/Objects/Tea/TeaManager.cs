using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Game.ServeTheQueen
{
    public class TeaManager : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Events.UnityEvent E_PositionningLimits;
        [SerializeField] private UnityEngine.Events.UnityEvent OnLoose;
        [SerializeField] private Animator anim;
        [SerializeField] private DatasTea teaDatas;
        [SerializeField] private Color colorLimitMin;
        private int numberTriggered;

        void Start() => teaDatas.hasWin = false;


        [Button("CheckReferences")]
        private void CheckReferences()
        {
            anim = GetComponent<Animator>();


        }

        public void LimitsPositionning() => E_PositionningLimits.Invoke();


        public void RandomizeLimits()
        {
            teaDatas.limitMin = Random.Range(.3f, .99f);

            if (Macro.Difficulty == 1)
            {
                Debug.Log("Hello25");
                teaDatas.limitMax = teaDatas.limitMin + teaDatas.offset;
            } else
            {
                Debug.Log("Hello");
                teaDatas.limitMax = teaDatas.limitMin + (teaDatas.offset * .5f);
            }

        }

        public void CreateVisualLimits()
        {

            teaDatas.prefabLimitMin.localPosition = new Vector3(0, teaDatas.limitMin - .5f, 0);
            Instantiate(teaDatas.prefabLimitMin, transform.parent);

            if (Macro.Difficulty == 1)
            {
                teaDatas.prefabLimitMax.localPosition = teaDatas.prefabLimitMin.localPosition + new Vector3(0, teaDatas.offset, 0);

            }
            else
            {
                teaDatas.prefabLimitMax.localPosition = teaDatas.prefabLimitMin.localPosition + new Vector3(0, teaDatas.offset * .5f, 0);
            }
                
            Instantiate(teaDatas.prefabLimitMax, transform.parent);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            numberTriggered++;
            
            if (numberTriggered == 1)
            {
                teaDatas.hasWin = true;
                collision.GetComponent<SpriteRenderer>().color = colorLimitMin;
            }
            else
            {
                if (!teaDatas.hasLoose)
                {

                    teaDatas.hasWin = false;
                    teaDatas.hasLoose = true;
                    OnLoose.Invoke();
                }


            }
        }



    }
}

