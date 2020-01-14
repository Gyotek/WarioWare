using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityAtoms;


public class ModeLibreSetup : MonoBehaviour
{
    [SerializeField]
    private IntVariable difficulty;

    [SerializeField]
    private IntVariable speed;

    public void DifficultyChoice(int newDifficulty)
    {
        difficulty.Value = newDifficulty;
        difficulty.SetValue(newDifficulty);
    }

    public void SpeedChoice(int newSpeed)
    {
        speed.Value = newSpeed;
        speed.SetValue(newSpeed);
    }
}
