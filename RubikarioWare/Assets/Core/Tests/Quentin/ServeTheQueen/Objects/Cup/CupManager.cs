using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms;

namespace Game.ServeTheQueen
{
    public class CupManager : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Events.UnityEvent OnTeaInCup;
        [SerializeField] private UnityEngine.Events.UnityEvent OnPourTeaStop;

        [SerializeField] private BoolVariable StatePourTea;
        [SerializeField] private float timerPourTea;
        [SerializeField] private DatasTea teaDatas;
        [SerializeField] private UnityEngine.Events.UnityEvent OnTeaOutside;
        private float timerOffset = -1;
        bool statePourTea;

        private void Update()
        {
            StartTimer();
        }
        public void StartTimer()
        {
            if (Macro.Difficulty != 3)
            {
                if (StatePourTea.Value) StartCoroutine("WaitPourTeaInCup");
                else
                {
                    StopCoroutine("WaitPourTeaInCup");
                    OnPourTeaStop.Invoke();
                }

            }
            else if (Macro.Difficulty == 3)
            {
                if (StatePourTea.Value )
                {
                    if (transform.localPosition.x >= -1 && transform.localPosition.x <= 1 && !statePourTea)
                    {
                        StartCoroutine("WaitPourTeaInCup");
                        statePourTea = true;
                        Debug.Log("true");
                    }
                    else if (transform.localPosition.x > 1 && statePourTea)
                    {
                        StopCoroutine("WaitPourTeaInCup");
                        OnPourTeaStop.Invoke();
                        statePourTea = false;
                        teaDatas.hasLoose = true;
                        OnTeaOutside.Invoke();
                        Debug.Log("false");
                    }

                    else if (transform.localPosition.x < -1  && statePourTea)
                    {
                        StopCoroutine("WaitPourTeaInCup");
                        OnPourTeaStop.Invoke();
                        statePourTea = false;
                        teaDatas.hasLoose = true;
                        OnTeaOutside.Invoke();
                        Debug.Log("false");
                    }

                }
                else
                {
                    StopCoroutine("WaitPourTeaInCup");
                    OnPourTeaStop.Invoke();
                    statePourTea = false;
                }
            }


        }


        IEnumerator WaitPourTeaInCup()
        {
            yield return new WaitForSeconds(timerPourTea);
            OnTeaInCup.Invoke();

        }

        public void PlaySoundOnce(AudioSource source)
        {
            if (!source.isPlaying) source.Play();
        }

    }

}

