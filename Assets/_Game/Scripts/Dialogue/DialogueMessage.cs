using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Dialogue
{
    public class DialogueMessage : MonoBehaviour
    {
        public CanvasGroup canvasGroup;
        public TextMeshProUGUI tmp;

        private void OnEnable()
        {
            canvasGroup.alpha = 1;
            canvasGroup.DOFade(0, 4f).SetDelay(2f).OnComplete(() => { Destroy(gameObject); });
        }
    }
}