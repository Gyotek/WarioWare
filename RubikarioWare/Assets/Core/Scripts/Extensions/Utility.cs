using System;
using  System.Linq;
using System.Collections;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

using Random = UnityEngine.Random;

namespace Game.Core
{
    public static class Utility
    {
        public static T GetRandomFlag<T>(int count) where T : Enum
        {
            if (count > 1)
            {
                var stringedValues = Enum.GetNames(typeof(T)).ToList();

                count = (int) Mathf.Clamp(count, 0, stringedValues.Count);
                stringedValues.RemoveAt(0);

                var stringedValue = string.Empty;
                for (var i = 0; i < count; i++)
                {
                    var randomIndex = Random.Range(0, stringedValues.Count);
                    stringedValue += $"{stringedValues[randomIndex]}, ";
                    stringedValues.RemoveAt(randomIndex);
                }

                stringedValue = stringedValue.Remove(stringedValue.Length - 2);
                return (T) Enum.Parse(typeof(T), stringedValue);
            }
            else return GetRandomEnumValue<T>();
        }

        public static T GetRandomEnumValue<T>() where T : Enum
        {
            var values = Enum.GetValues(typeof(T)).Convert(obj => (T)obj).ToArray();
            var randomIndex = Random.Range(0, values.Length);
                
            return values[randomIndex];
        }
        
        public static IEnumerable<T> Split<T>(this T enumValue) where T : Enum
        {
            return Enum.GetValues(typeof(T))
                .Convert(obj =>(T)obj)
                .Where(value => Convert.ToInt32(value) != 0 && enumValue.HasFlag(value));
        }

        public static void Shuffle<T>(this IList<T> collection)
        {
            var n = collection.Count;
            for (var i = collection.Count - 1; i > 1; i--)
            {
                var randIndex = Random.Range(0, n + 1);
                var randItem = collection[randIndex];
                collection[randIndex] = collection[i];
                collection[i] = randItem;
            }
        }
    }
}

