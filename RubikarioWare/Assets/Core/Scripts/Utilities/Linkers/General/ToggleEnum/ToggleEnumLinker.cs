using System;
using System.Collections.Generic;
using System.Linq;

using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public abstract class ToggleEnumLinker<T> : SerializedMonoBehaviour where T : Enum
{
    #region EncapsuledTypes

    [Serializable]
    private struct ToggleEnumPair
    {
        [SerializeField] private Toggle toggle;
        public Toggle Toggle => toggle;

        [SerializeField] private T associatedEnum;
        public T AssociatedEnum => associatedEnum;
    }

    #endregion

    [SerializeField] private ToggleEnumPair[] pairs;
    [SerializeField] private UnityEvent onEnumValueChanged;
    public UnityEvent OnEnumValueChanged => onEnumValueChanged;

    private bool isFlagged;
    [ShowInInspector, ReadOnly] protected T flaggedEnumValue;
    public T FlaggedEnumValue => flaggedEnumValue;

    [ShowInInspector, ReadOnly] private List<T> enumValues = new List<T>();
    public IReadOnlyCollection<T> EnumValues => enumValues;

    void Awake()
    {
        isFlagged = typeof(T).GetCustomAttribute<FlagsAttribute>() != null;
        
        RefreshEnumValue();
        foreach (var pair in pairs) pair.Toggle.onValueChanged.AddListener(isOn => RefreshEnumValue());
    }
    
    protected void RefreshEnumValue()
    {
        if (isFlagged)
        {
            if (pairs.Any(pair => pair.Toggle.isOn))
            {
                var stringedEnumValue = pairs.Where(pair => pair.Toggle.isOn)
                    .Aggregate(string.Empty, (current, pair) => current + $"{pair.AssociatedEnum.ToString()}, ");

                stringedEnumValue = stringedEnumValue.Remove(stringedEnumValue.Length - 2);
                flaggedEnumValue = (T)Enum.Parse(typeof(T), stringedEnumValue);
            }
            else SetDefaultFlag();
        }
        else
        {
            enumValues.Clear();
            foreach (var pair in pairs.Where(pair => pair.Toggle.isOn)) enumValues.Add(pair.AssociatedEnum);
        }
        
        onEnumValueChanged.Invoke();
    }

    private void SetDefaultFlag() => flaggedEnumValue = (T) Enum.Parse(typeof(T), Enum.GetNames(typeof(T))[0]);
}