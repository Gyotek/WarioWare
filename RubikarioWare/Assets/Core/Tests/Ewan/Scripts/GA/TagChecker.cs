using UnityAtoms;
//using UnityAtoms.Tags;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
	public class TagChecker : MonoBehaviour
    {
        StringConstant stringConstant;

        public UnityEvent OnHasTag = default;

        public void Check(GameObject gameObject)
        {
            /*var atomTags = gameObject.GetComponent<AtomTags>();
            if (atomTags.HasTag(stringConstant.Value))
            {
                OnHasTag?.Invoke();
            }*/
        }
    }
}