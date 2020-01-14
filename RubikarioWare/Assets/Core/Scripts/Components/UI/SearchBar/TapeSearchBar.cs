using System.Linq;
using Game.Core;
using UnityEngine;
using UnityEngine.UI;

public class TapeSearchBar : SearchBar<TapeSearchArguments>
{
    [SerializeField] private InputField inputField;
    
    [SerializeField] private ToggleRivalsLinker toggleRivalsLinker;
    [SerializeField] private ToggleInputsLinker toggleInputsLinker;
    [SerializeField] private ToggleThemeLinker toggleThemeLinker;

    void Start()
    { 
        arguments = new TapeSearchArguments(
            this, 
            string.Empty, 
            toggleRivalsLinker.FlaggedEnumValue,
            toggleInputsLinker.FlaggedEnumValue,
            toggleThemeLinker.EnumValues.ToArray());  
        
        toggleRivalsLinker.OnEnumValueChanged.AddListener(RefreshFilters);
        toggleInputsLinker.OnEnumValueChanged.AddListener(RefreshFilters);
        toggleThemeLinker.OnEnumValueChanged.AddListener(RefreshFilters);
        
        inputField.onValueChanged.AddListener(RefreshSearchedContent);
    }
    
    public void RefreshSearchedContent(string searchedContent)
    {
        arguments = new TapeSearchArguments(
            this, 
            searchedContent, 
            arguments.Rivals,
            arguments.Inputs,
            arguments.Themes);
        
        Search();
    }
    public void RefreshFilters()
    {
        arguments = new TapeSearchArguments(
            this,
            arguments.SearchedContent, 
            toggleRivalsLinker.FlaggedEnumValue,
            toggleInputsLinker.FlaggedEnumValue,
            toggleThemeLinker.EnumValues.ToArray());
        
        Search();
    }
}