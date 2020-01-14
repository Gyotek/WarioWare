using System.Collections.Generic;

using UnityAtoms;
using UnityEngine;

namespace Game.Core
{
    public abstract class AtomLinker<T> : MonoBehaviour where T : Object
    {
        [SerializeField] protected T value;
        [SerializeField] protected List<AtomBaseVariable> atomVariables = default;
    }
}