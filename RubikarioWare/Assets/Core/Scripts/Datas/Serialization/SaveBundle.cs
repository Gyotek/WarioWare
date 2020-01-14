using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


using UnityEngine;

using Sirenix.OdinInspector;

using Object = UnityEngine.Object;

namespace Game.Core.Serialization
{
    public class SaveBundle : MonoBehaviour
    {
        [SerializeField] private string bundleName;
        public string Name => bundleName;
        private string Path => $"{Application.persistentDataPath}/{bundleName}.json";
        
        [ListDrawerSettings(OnTitleBarGUI = "VerifyObjects")]
        [SerializeField] private Object[] objectsToSave = default;
        private Dictionary<int, int> indices = new Dictionary<int, int>();

        #if  UNITY_EDITOR
        
        private void VerifyObjects()
        {
            if (!GUILayout.Button("Verify")) return;
            
            var objectList = objectsToSave.ToList();
            objectList.RemoveAll(obj => !(obj is ISavable));
            objectsToSave = objectList.ToArray();
        }
        
        #endif

        void Awake()
        {
            SaveSystem.RegisterBundle(this);
            for (var i = 0; i < objectsToSave.Length; i++) indices.Add(objectsToSave[i].GetInstanceID(), i);
            
            if (!File.Exists(Path)) CreateSaveFile();
        }
        
        [Button]
        public void Save(int id)
        {
            if (!indices.TryGetValue(id, out var index)) return;
            
            var content = File.ReadAllText(Path);
            var startIndex = content.IndexOf($"Ⓓ{index}") + 2;

            if (index + 1 == objectsToSave.Length) content = content.Remove(startIndex);
            else
            {
                var endIndex =  content.IndexOf($"Ⓓ{index + 1}");
                content = content.Remove(startIndex, endIndex - startIndex);
            }
            
            var dataGroup = ((ISavable) objectsToSave[index]).Serialize();
            var dataContent = new StringBuilder();
            foreach (var data in dataGroup) dataContent.Append($"➤{data}");

            content = content.Insert(startIndex, dataContent.ToString());
            File.WriteAllText(Path, content);
        }

        [Button]
        public void Load(int id)
        {
            if (!indices.TryGetValue(id, out var index)) return;
            
            var content = File.ReadAllText(Path);
            var dataGroup = content.Split(new char[]{'Ⓓ'}, StringSplitOptions.RemoveEmptyEntries)[index];
            dataGroup = dataGroup.Remove(0, 1);
            
            var data = dataGroup.Split(new char[]{'➤'}, StringSplitOptions.RemoveEmptyEntries);
            ((ISavable)objectsToSave[index]).Deserialize(data);
        }

        [Button]
        private void CreateSaveFile()
        {
            var content = new StringBuilder();
            for (var i = 0; i < objectsToSave.Length; i++)
            {
                content.Append($"Ⓓ{i}");

                var savable = objectsToSave[i] as ISavable;
                var subContents = savable.Serialize();

                foreach (var subContent in subContents) content.Append($"➤{subContent}");
            }
            File.WriteAllText(Path, content.ToString());
        }
    }
}