using System.Collections.Generic;

using Sirenix.OdinInspector;

using UnityEngine;

public abstract class SearchBar<T> : MonoBehaviour where T : SearchArguments
{
    private List<ISearchable<T>> searchables = new List<ISearchable<T>>();
    public List<ISearchable<T>> Searchables => searchables;
    
    [ShowInInspector, ReadOnly] protected T arguments;

    protected void Search()
    {
        foreach (var searchable in searchables)
        {
            if (searchable.IsMatch(arguments))  searchable.Show();
            else searchable.Hide();
        }
    }
}