using System;
using System.Collections.Generic;
using System.Reflection.Emit;

using UnityAtoms;
using UnityEngine;

namespace Game.Core
{
    public abstract class AtomTextLinker<T> : AtomLinker<T> where T : MonoBehaviour
    {
        [SerializeField] protected string format;
        
        protected List<Func<AtomBaseVariable, object>> functions = new List<Func<AtomBaseVariable, object>>();
        
        
        protected virtual void Awake()
        {
            while (atomVariables.Contains(null))
            {
                for (var i = 0; i < atomVariables.Count; i++)
                {
                    if (atomVariables[i] != null) continue;
                    atomVariables.RemoveAt(i);
                    break;
                }
            }
            foreach (var atomVariable in atomVariables) CreateFunction(atomVariable);
        }

        public abstract void Refresh();
        
        protected string GetText()
        {
            var args = new object[functions.Count];
            for (var i = 0; i < functions.Count; i++)
            {
                args[i] = functions[i](atomVariables[i]);
            }
            return string.Format(format, args);
        }
        private void CreateFunction(AtomBaseVariable atomVariable)
        {
            var atomType = atomVariable.BaseValue.GetType();
            var atomGetter = atomVariable.GetType().GetProperty("Value").GetMethod;

            var parameters = new Type[] { typeof(AtomBaseVariable) };
            var method = new DynamicMethod($"GetAtomValue.{GetInstanceID()}", typeof(object), parameters);
            var generator = method.GetILGenerator();
            
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Callvirt, atomGetter);
            generator.Emit(OpCodes.Box, atomType);
            generator.Emit(OpCodes.Ret);
            
            functions.Add((Func<AtomBaseVariable, object>) method.CreateDelegate(typeof(Func<AtomBaseVariable, object>)));
        }
    }
}