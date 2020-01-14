using UnityEngine;
using TMPro;

namespace Game
{
    public class VersionTextLinker : MonoBehaviour
    {
        [SerializeField] TMP_Text textMesh = default;

        private void Awake() => textMesh?.SetText(Application.version);
    }
}