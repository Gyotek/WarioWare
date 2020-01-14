using Game.Core;
using UnityEngine;
using Sirenix.OdinInspector;

public class TapeSearchArguments : SearchArguments
{
    public TapeSearchArguments(Object source, string searchedContent, Rivals rivals, Inputs inputs, Theme[] themes) : base(source, searchedContent)
    {
        this.rivals = rivals;
        this.inputs = inputs;
        this.themes = themes;
    }

    [ShowInInspector, ReadOnly] private Rivals rivals;
    public Rivals Rivals => rivals;

    [ShowInInspector, ReadOnly] private Inputs inputs;
    public Inputs Inputs => inputs;

    [ShowInInspector, ReadOnly] private Theme[] themes;
    public Theme[] Themes => themes;
}