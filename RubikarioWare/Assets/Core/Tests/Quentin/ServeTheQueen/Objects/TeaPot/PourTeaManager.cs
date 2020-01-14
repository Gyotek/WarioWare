using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms;

namespace Game.ServeTheQueen
{
    public class PourTeaManager : MicroMonoBehaviour
    {
        [SerializeField] private BoolVariable StatePourTea;
        [SerializeField] private DatasTea teaDatas;

        // VOir animation event 
        public bool canPourTea = false;
        bool hasFinish = false;

        // Update is called once per frame

        void Update()
        {


            if(Macro.RemainingTime > 5 && !hasFinish)
            {
                hasFinish = true;
                
            }
            else if (Macro.RemainingTime <= 5 && !hasFinish)
            {
                ChangeStatePourTea();
            }

            if (!StatePourTea.Value && teaDatas.hasWin)
            {
                if (!hasFinish)
                {
                    hasFinish = true;
                }
           

            }


        }

        // Call par une animation event de la théière
        public void ChangeCanPourTeaTrue() => canPourTea = true;
        // Call par une animation event de la théière
        public void ChangeCanPourTeaFalse() => canPourTea = false;

        void ChangeStatePourTea()
        {


            if (Input.GetKey(KeyCode.Space) && canPourTea)
            {
                if (!StatePourTea.Value) StatePourTea.Value = true;
            }

            else if (Input.GetKeyUp(KeyCode.Space))
            {
                StatePourTea.Value = false;
                canPourTea = StatePourTea.Value;
            }

      
        }

        public void SetBoolAnimator(Animator anim)
        {
            anim.SetBool("CanPourTea", StatePourTea.Value);
        }

        public void SetTeaParticles(ParticleSystem teaParticles)
        {
            if (StatePourTea.Value)
            {
                teaParticles.gravityModifier = 1;
                teaParticles.Play();
            } 
            else
            {
                teaParticles.gravityModifier = 20;
                teaParticles.Stop();
            } 
        }


    }
}


