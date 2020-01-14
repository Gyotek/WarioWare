using System.Collections;
using UnityAtoms;

public interface ISearchable<T> where T : SearchArguments
{
    bool IsMatch(T arguments);

    void Show();
    void Hide();
}