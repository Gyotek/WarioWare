using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.ScratchyBird
{
    public class TubesSpawner : MonoBehaviour
    {
        [SerializeField] private Transform firstTube;
        [SerializeField] private Transform distance0;
        [SerializeField] private Transform distance3;
        [SerializeField] private Transform distance5;
        [SerializeField] private Transform tubeLeft;
        [SerializeField] private Transform tubeRight;
        [SerializeField] private Transform hauteurD1haut;
        [SerializeField] private Transform hauteurD1bas;
        [SerializeField] private Transform hauteurD2haut;
        [SerializeField] private Transform hauteurD2bas;
        [SerializeField] private Transform hauteurD3haut;
        [SerializeField] private Transform hauteurD3bas;


        [SerializeField] private Transform tubesParent;
        [SerializeField] private GameObject tubePrefab;
        [SerializeField] private GameObject endTube1Prefab;
        [SerializeField] private GameObject endTube2Prefab;
        [SerializeField] private GameObject endTube3Prefab;
        [SerializeField] private GameObject endTube4Prefab;

        private int spawnNumber;
        private float distance;
        private float tube;
        private float distanceMin;
        private float distanceMax;


        // Start is called before the first frame update
        void Start()
        {
            tube = Vector3.Distance(tubeLeft.position, tubeRight.position);
            distanceMin = Vector3.Distance(distance0.position, distance3.position);
            distanceMax = Vector3.Distance(distance0.position, distance5.position);

            distance = tube;

            if (Macro.Difficulty == 1)
                Difficulté1();
            else if (Macro.Difficulty == 2)
                Difficulté2();
            else if (Macro.Difficulty == 3)
                Difficulté3();

        }

        void Difficulté1()
        {
            float x = 0;
            float y = 0;

            spawnNumber = Random.Range(1, 2);

            for (int i = 0; i < spawnNumber; i++)
            {
                x += distance + Random.Range(distanceMin, distanceMax);
                y = Random.Range(hauteurD1bas.position.y, hauteurD1haut.position.y);
                if (i == spawnNumber - 1)
                    Instantiate(endTube1Prefab, new Vector2(x, y), Quaternion.identity, tubesParent);
                else
                    Instantiate(tubePrefab, new Vector2(x, y), Quaternion.identity, tubesParent);
            }
        }

        void Difficulté2()
        {
            float x = 0;
            float y = 0;

            spawnNumber = Random.Range(2, 3);

            for (int i = 0; i < spawnNumber; i++)
            {
                x += distance + Random.Range(distanceMin, distanceMax);
                y = Random.Range(hauteurD2bas.position.y, hauteurD2haut.position.y);
                if (i == spawnNumber - 1)
                    Instantiate(endTube2Prefab, new Vector2(x, y), Quaternion.identity, tubesParent);
                else
                    Instantiate(tubePrefab, new Vector2(x, y), Quaternion.identity, tubesParent);
            }
        }

        void Difficulté3()
        {
            float x = 0;
            float y = 0;

            spawnNumber = 3;

            for (int i = 0; i < spawnNumber; i++)
            {
                x += distance + Random.Range(distanceMin, distanceMax);
                y = Random.Range(hauteurD3bas.position.y, hauteurD3haut.position.y);
                if (i == spawnNumber - 1)
                {
                    int random = Random.Range(1, 3);
                    if (random == 1)
                        Instantiate(endTube3Prefab, new Vector2(x, y), Quaternion.identity, tubesParent);
                    else
                        Instantiate(endTube4Prefab, new Vector2(x, y), Quaternion.identity, tubesParent);
                }
                else
                    Instantiate(tubePrefab, new Vector2(x, y), Quaternion.identity, tubesParent);
            }
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}
