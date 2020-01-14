using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Game.Core;
using Game.Core.Serialization;

namespace Game.Playlist
{
    public class PlaylistManager : MonoBehaviour, ISavable
    {
        [SerializeField] private AssetGroup gameIds;
        [SerializeField] private StorySequence sequence;
        [SerializeField] private GameSequenceHandler gameSequenceHandle;

        public static List<GameID> currentList = new List<GameID>();
        public static List<int> difficulty = new List<int>();
        public static List<ModifyGameIdPlaylist> icones = new List<ModifyGameIdPlaylist>();

        private string[] savedFiles = new string[1];
        private string code;

        private bool isRandom;

        public static bool CanAddGame()
        {
            if (currentList.Count < 30)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void AddGame(GameID id, ModifyGameIdPlaylist modif)
        {
            currentList.Add(id);
            difficulty.Add(1);
            icones.Add(modif);
        }

        public static void RemoveGame(int emplacement)
        {
            Debug.Log(emplacement);
            currentList.RemoveAt(emplacement);
            difficulty.RemoveAt(emplacement);
            foreach(ModifyGameIdPlaylist mod in icones)
            {
                if(emplacement < mod.emplacementList)
                {
                    mod.emplacementList--;
                }
            }
            icones.RemoveAt(emplacement);
        }

        public static void ChangeDifficulty(int emplacement)
        {
            difficulty[emplacement] = (difficulty[emplacement] + 1) % 3;
        }

        private void SequenceCreation()
        {
            SaveSystem.LoadData(GetInstanceID(), "Playlist");
            sequence.SetSequence(CreateListFromString(code).ToArray());
        }

        public void ChangeRandom()
        {
            isRandom = !isRandom;
        }

        public void PlayPlaylist()
        {
            SequenceCreation();
            gameSequenceHandle.StartNewSequence(sequence as IGameSequence);
        }

        public void SavePlaylist()
        {
            CreatePlaylistCode();
            SaveSystem.SaveData(GetInstanceID(), "Playlist");
        }

        void CreatePlaylistCode()
        {
            code = "";
            for(int i = 0; i < currentList.Count; i++)
            {
                string[] name = currentList[i].name.Split('_');
                code += name[0];
                //code += "_";
                //code += difficulty[i];
                code += ";";
            }
            Debug.Log("Code : " + code);
        }

        List<GameID> CreateListFromString(string code)
        {
            string[] ids = code.Split(';');
            List<GameID> newList = new List<GameID>();
            List<GameID> idsList = gameIds.GetAssets<GameID>().ToList();

            foreach (GameID id in idsList)
            {
                string[] num = id.name.Split('_');

                foreach (string str in ids)
                {
                    if(str == num[0])
                    {
                        newList.Add(id);
                    }
                }
            }
            return newList;
        }

        string[] ISavable.Serialize()
        {
            Debug.Log("Allo la save ?");
            savedFiles[0] = code;
            return savedFiles;
        }

        void ISavable.Deserialize(string[] data)
        {
            code = data[0];
        }
    }
}
