using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;

namespace Game.Temporary
{
    public class SaveSystem : MonoBehaviour
    {
        static string GetPath(string name, string extension = ".txt") => Path.Combine(Application.persistentDataPath, name + extension);

        public static void Save(object objToSave, string name)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(GetPath(name));
            var json = JsonUtility.ToJson(objToSave);
            bf.Serialize(file, json);
            file.Close();
            Debug.Log("Did Save");
        }

        public static bool Load(object objToLoad ,string name)
        {
            BinaryFormatter bf = new BinaryFormatter();
            if(File.Exists(GetPath(name)))
            {
                FileStream file = File.Open(GetPath(name), FileMode.Open);
                JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), objToLoad);
                file.Close();
                return true;
            }
            return false;
        }
    }
}