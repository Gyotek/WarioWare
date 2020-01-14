using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class ColorSuperTextLinker : MonoBehaviour
    {
        public Color color;
        [SerializeField] private SuperTextMesh superTextMesh;

        private void Update()
        {
            superTextMesh.color = color;
            superTextMesh.Rebuild();
        }
    }
}
