using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms;

namespace Game.ScratchyBird
{ 
    public class Move : MonoBehaviour
    {

        private bool isMoving = true;
        [SerializeField] private ScratchyController scratchy;

        public void OnGameStoped() =>
            isMoving = false;

        void Update()
        {
            if(isMoving)
                transform.position -= Vector3.right * scratchy.speed * Time.deltaTime;

        }

    }
}

