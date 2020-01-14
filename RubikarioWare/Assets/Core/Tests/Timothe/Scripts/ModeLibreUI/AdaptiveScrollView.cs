using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [ExecuteInEditMode]
    public class AdaptiveScrollView : MonoBehaviour
    {
        private void Update()
        {
            /*RectTransform rect = gameObject.GetComponent<RectTransform>();
            if (gameObject.transform.childCount > 12)
            {
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, 610 + (150 * Mathf.Ceil(((gameObject.transform.childCount - 10) / 3))));
            }
            else
            {
                rect.sizeDelta = new Vector2(rect.sizeDelta.x, 610);
            }*/
        }
    }
}
