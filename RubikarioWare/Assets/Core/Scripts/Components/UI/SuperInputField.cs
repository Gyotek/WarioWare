using System.Collections;
using System.Linq;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Core
{
    public class SuperInputField : MonoBehaviour
    {
        [SerializeField] private InputField inputField;
        
        [Space(15)]
        
        [SerializeField] private SuperTextMesh sampleText;
        [SerializeField] private SuperTextMesh inputText;

        [Space(15)] 
        
        [SerializeField] private Image caret;
        private float CaretY => caret.rectTransform.anchoredPosition.y;
        
        [Space(15)]
        
        [SerializeField] private Image selectionLine;
        private float LineY => selectionLine.rectTransform.anchoredPosition.y;

        [SerializeField] private float horizontalLineOffset;
        [SerializeField] private float blinkingTime;
        private float desiredAlpha;

        void Start()
        {
            desiredAlpha = selectionLine.color.a;
            StartCoroutine(BlinkingRoutine());
        }
        void Update()
        {
            if (inputField.text == string.Empty)
            {
                SetCaret(0, 0);
                selectionLine.enabled = false;
                
                return;
            }
            
            var start = inputField.selectionAnchorPosition;
            var end = inputField.selectionFocusPosition;
            
            if (start == end)
            {
                SetCaret(0, 0);
                
                selectionLine.enabled = true;
                var x = 0f;
                for (var i = 0; i < start; i++) x +=  inputText.info[i].ch.advance * 0.5f;
                selectionLine.rectTransform.anchoredPosition = new Vector2(x + horizontalLineOffset, LineY);
            }
            else
            {
                selectionLine.enabled = false;
                
                var length = end - start;
                var width = 0f;
                var x = 0f;
            
                if (length > 0)
                {
                    for (var i = start; i < end; i++) width += inputText.info[i].ch.advance * 0.5f;
                    for (var i = 0; i < start; i++) x +=  inputText.info[i].ch.advance * 0.5f;
                }
                else
                {
                    for (var i = start - 1; i >= end; i--) width += inputText.info[i].ch.advance * 0.5f;
                    for (var i = 0; i < end; i++) x += inputText.info[i].ch.advance * 0.5f;
                }
                SetCaret(x, width);
            }
            
            void SetCaret(float xPos, float rectWidth)
            {
                caret.rectTransform.anchoredPosition = new Vector2(xPos, CaretY);
                caret.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectWidth);
            }
        }

        private IEnumerator BlinkingRoutine()
        {
            yield return new WaitForSeconds(blinkingTime);
            
            var color = selectionLine.color;
            color.a = 0;
            selectionLine.color = color;
            
            yield return new WaitForSeconds(blinkingTime);
            
            color.a = desiredAlpha;
            selectionLine.color = color;

            StartCoroutine(BlinkingRoutine());
        }
        
        public void Refresh()
        {
            if (inputField.text != string.Empty)
            {
                sampleText.enabled = false;
                inputText.text = inputField.text;
            }
            else
            {
                sampleText.enabled = true;
                inputText.text = string.Empty;
            }
        }
    }
}

