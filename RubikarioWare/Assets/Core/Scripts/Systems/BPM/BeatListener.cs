using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using Sirenix.OdinInspector;
using UnityAtoms;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Core;

public class BeatListener : MonoBehaviour
{
    private BeatEngine beat;
    [ShowInInspector, ReadOnly]
    private List<double> delays = new List<double>();
    [ShowInInspector, ReadOnly]
    private double averageDelay {
        get
        {
            if (delays.Count > 0) return delays.Average();
            else return 0;
        }}

    public void OnBeat()
    {
    }

    public void OnActionKey(InputAction.CallbackContext context)
    {
        bool onKeyDown = context.started;
        bool onKey = context.performed;
        bool onKeyUp = context.canceled;
        
        Debug.LogFormat("OnKeyDown : {0} - OnKey : {1} - OnKeyUp : {2}", onKeyDown, onKey, onKeyUp);
        if (!BeatEngine.IsPaused && context.started)
        {
            double closest = BeatEngine.TimeBeforeNext < BeatEngine.TimeSinceLast ? BeatEngine.TimeBeforeNext : BeatEngine.TimeSinceLast;
            delays.Add(closest);
            Debug.Log("Delay: " + closest + "/ Average: " + averageDelay);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Debug.Log(context.ReadValue<Vector2>());
    }
}
