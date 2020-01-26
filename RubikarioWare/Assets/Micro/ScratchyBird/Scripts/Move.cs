using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms;

namespace Game.ScratchyBird
{ 
    public class Move : MonoBehaviour
    {

        private bool isMoving = false;
        [SerializeField] private ScratchyController scratchy;

        public void OnGameStoped() =>
            isMoving = false;
        
        public void StartMoving()
        {
            isMoving = true;
        }

        void Update()
        {
            if(isMoving && !ScratchyBirdGameManager.instance.gameEnded)
                transform.position -= Vector3.right * scratchy.speed * Time.deltaTime;

        }

    }
}

