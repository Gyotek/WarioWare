using UnityEngine;

namespace Game.Core
{
    public abstract class ExplicitAssetGroup<T> : ScriptableObject where T : UnityEngine.Object
    {
        [SerializeField] protected T[] group;
        public T[] Group => group;
    }
}