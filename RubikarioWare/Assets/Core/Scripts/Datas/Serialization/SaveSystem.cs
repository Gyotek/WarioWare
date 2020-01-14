using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Serialization
{
    public static class SaveSystem
    {
        private static Dictionary<string, SaveBundle> saveBundles = new Dictionary<string, SaveBundle>();

        public static void RegisterBundle(SaveBundle bundle)
        {
            if (!saveBundles.ContainsKey(bundle.Name)) saveBundles.Add(bundle.Name, bundle);
        }

        public static void SaveData(int id, string bundleKey)
        {
            if (saveBundles.TryGetValue(bundleKey, out var bundle)) bundle.Save(id);
        }
        public static void LoadData(int id, string bundleKey)
        {
            if (saveBundles.TryGetValue(bundleKey, out var bundle)) bundle.Load(id);
        }
    }
}