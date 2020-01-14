using System.Collections;
using System.Globalization;
using UnityAtoms;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameInfoContextLinker : MonoBehaviour
    {
        [SerializeField] private FloatVariable timeGoalAtom;
        [SerializeField] private FloatVariable timeAdvancementAtom;

        [Space] 
        
        [SerializeField] private Image winIndicator;
        [SerializeField] private Color[] winColors;

        [Space] 
        
        [SerializeField] private SuperTextMesh textMesh;
        [SerializeField] private float refreshTime;
        
        private Coroutine refreshRoutine;

        public void ResetWinIndicator() => winIndicator.color = winColors[0];
        public void SetWinIndicatorToWon() => winIndicator.color = winColors[1];
        public void SetWinIndicatorToLost() => winIndicator.color = winColors[2];

        public void StartTimerLink()
        {
            if (refreshRoutine != null) return;
            refreshRoutine = StartCoroutine(RefresRoutine());
        }

        public void EndTimerLink()
        {
            if (refreshRoutine == null) return;
            
            StopCoroutine(refreshRoutine);
            refreshRoutine = null;

            textMesh.text = "00.00";
        }
        
        private IEnumerator RefresRoutine()
        {
            var remainingTime = timeGoalAtom.Value - timeAdvancementAtom.Value;

            if (remainingTime % 1 == 0) textMesh.Text = remainingTime < 10 ? $"0{remainingTime}.00" : $"{remainingTime}.00";
            else if (remainingTime < 0) textMesh.Text = "00.00";
            else
            {
                var splittedValue = remainingTime.ToString(CultureInfo.InvariantCulture).Split('.');

                if (splittedValue[0].Length == 1) splittedValue[0] = $"0{splittedValue[0]}";
                
                if (splittedValue[1].Length > 2) splittedValue[1] = splittedValue[1].Remove(2);
                else if (splittedValue[1].Length == 1) splittedValue[1] += "0";
                
                textMesh.Text = $"{splittedValue[0]}.{splittedValue[1]}";
            }
            
            yield return new WaitForSeconds(refreshTime);
            refreshRoutine = StartCoroutine(RefresRoutine());
        }
    }
}