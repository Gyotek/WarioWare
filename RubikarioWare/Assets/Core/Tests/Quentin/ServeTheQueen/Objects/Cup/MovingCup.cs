using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.ServeTheQueen

{
    public class MovingCup : MicroMonoBehaviour
    {
        [SerializeField] private UnityEngine.Events.UnityEvent E_MoveTheCupRight;
        [SerializeField] private UnityEngine.Events.UnityEvent E_MoveTheCupLeft;
        [SerializeField] private UnityEngine.Events.UnityEvent E_ShakeEvent;
        [SerializeField] private Animator anim;
        [SerializeField] private CupDatas cupDatas;
        private float timer = 1;
        private bool isMoving;
        private bool moveRight;
        private float y;


        protected override void OnGameStart()
        {
            if(Macro.Difficulty == 3) StartCoroutine(WaitMoveStart());
        }
        private void Update()
        {
            if (Macro.Difficulty == 3)
            {
                if (timer < Time.time  && !isMoving)
                {
                    StartCoroutine(WaitMoveUpdate());
                }

            }

        }



        IEnumerator WaitMoveStart()
        {
            isMoving = true;
            E_ShakeEvent.Invoke();
            yield return new WaitForSeconds(1.5f);
            if (Random.value > .5f)
            {
                E_MoveTheCupRight.Invoke();
                moveRight = true;
            }

            else
            {
                E_MoveTheCupLeft.Invoke();
                moveRight = false;
            }
           
            yield return new WaitForSeconds(Random.Range(.5f,1f));
            SetReverseAndTimer();

        }

        IEnumerator WaitMoveUpdate()
        {
            isMoving = true;

            if (y >= cupDatas.timeBeforeSlideMin && y < (cupDatas.timeBeforeSlideMax * .8f))
            {
                yield return new WaitForSeconds(cupDatas.timerBeforeReverse -.5f);
                E_ShakeEvent.Invoke();

                yield return new WaitForSeconds(cupDatas.timerBeforeReverse);
                InvokeEvent();
  
            } else if (y >= (cupDatas.timeBeforeSlideMax *.8f) && y <= cupDatas.timeBeforeSlideMax)
            {

                float random = Random.Range(cupDatas.timerBeforeReverseMin, cupDatas.timerBeforeReverseMax);
                yield return new WaitForSeconds(random -.5f);
                E_ShakeEvent.Invoke();

                yield return new WaitForSeconds(random);
                InvokeEvent();
            }

            SetReverseAndTimer();

        }

        void SetReverseAndTimer()
        {

            anim.SetTrigger("Reverse");
            y = Random.Range(cupDatas.timeBeforeSlideMin, cupDatas.timeBeforeSlideMax);
            timer = Time.time + y;
            isMoving = false;


        }

        void InvokeEvent()
        {
            if (moveRight)
            {
                E_MoveTheCupLeft.Invoke();
                moveRight = false;
            }
            else
            {
                E_MoveTheCupRight.Invoke();
                moveRight = true;
            }
        }

    }
        

}
