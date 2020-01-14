using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Sirenix.OdinInspector;
using Sirenix.Utilities;

using UnityEngine;

namespace Game.Core
{
    [CreateAssetMenu(fileName = "AssetGroup", menuName = "Unity Atoms/Lists/Asset Group")]
    public class AssetGroup : ScriptableObject
    {
        [SerializeField, HideInInspector] private string typeString;
        [ShowInInspector, ReadOnly] public Type Type => Type.GetType(typeString);

        [SerializeField] private ScriptableObject[] group;
        public IEnumerable<T> GetAssets<T>() => typeof(T) == Type ? @group.Convert(so => (T) so) : null;

        public void SetGroupType<T>() where T : ScriptableObject
        {
            typeString = typeof(T).FullName;
            VerifyGroup();
        }
        public void SetGroupType(Type type)
        {
            if (!typeof(ScriptableObject).IsAssignableFrom(type)) return;
            
            typeString = type.FullName;
            VerifyGroup();
        }

        public void VerifyGroup()
        {
            var list = group.ToList();
            list.RemoveAll(so => so == null || so.GetType() != Type);
            group = list.ToArray();
        }
    }
}