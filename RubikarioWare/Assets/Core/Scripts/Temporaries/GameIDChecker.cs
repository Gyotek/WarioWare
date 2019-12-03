using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class GameIDChecker : MonoBehaviour
    {
        [SerializeField] private GameID gameID = default;
		// Start is called before the first frame update
		public GameID GameID => gameID;

		public static event Action<GameID> OnGameIDCheck = default;

        void Start()
        {
            Checking();
        }

        private void Checking()
        {
            if (gameID)
            {
                if (!gameID.Check()) Debug.Break();
				OnGameIDCheck?.Invoke(gameID);
			}
            else Debug.LogError("GameID Reference not found. Please add the GameID to " + gameObject.name + ".");
        }

    }
}