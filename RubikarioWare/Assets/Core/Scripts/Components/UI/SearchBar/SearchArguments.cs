using Sirenix.OdinInspector;
using UnityEngine;

public class SearchArguments
{
    public SearchArguments(Object source, string searchedContent)
    {
        this.source = source;
        this.searchedContent = searchedContent;
    }
    
    [ShowInInspector, ReadOnly] private Object source;
    public Object Source => source;

    [ShowInInspector, ReadOnly] private string searchedContent;
    public string SearchedContent => searchedContent;
}