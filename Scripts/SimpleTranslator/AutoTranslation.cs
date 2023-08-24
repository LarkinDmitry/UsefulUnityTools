using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KAKuBCE.UsefulUnityTools
{
    public class AutoTranslation : MonoBehaviour
    {
        void Start()
        {
            StringLocalizator.LanguageIsChanged += Translate;
            Translate();
        }

        private void OnDestroy()
        {
            StringLocalizator.LanguageIsChanged -= Translate;
        }

        private void Translate()
        {
            if (TryGetComponent(out Text text)) text.text = text.text.Translate();
            if (TryGetComponent(out TextMeshPro textMeshPro)) textMeshPro.text = textMeshPro.text.Translate();
            if (TryGetComponent(out TextMeshProUGUI textMeshProUGUI)) textMeshProUGUI.text = textMeshProUGUI.text.Translate();
        }
    }
}