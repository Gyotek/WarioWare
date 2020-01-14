using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
    [Serializable]
    public struct NumericDisplayConfig
    {
        public NumericDisplayConfig(int range, bool allowDecimals)
        {
            this.range = range;
            this.allowDecimals = allowDecimals;
        }

        [SerializeField, MinValue(1)] private int range;
        [SerializeField] private bool allowDecimals;

        public string Convert(float value)
        {
            var stringedValue = value.ToString(CultureInfo.InvariantCulture);
            var length = stringedValue.Contains('.') ? stringedValue.Length - 1 : stringedValue.Length;

            var results = new List<char>();
            if (length < range)  for (var i = 0; i < range - length; i++) results.Add('0');

            var count = 0;
            var index = 0;
            while (index < stringedValue.Length)
            {
                results.Add(stringedValue[index]);
                if(char.IsNumber(stringedValue[index])) count++;
                index++;
            }
            return new string(results.ToArray());
        }
    }
}