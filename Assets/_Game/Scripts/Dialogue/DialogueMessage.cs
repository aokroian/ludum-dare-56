using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Dialogue
{
    public class DialogueMessage : MonoBehaviour
    {
        public CanvasGroup canvasGroup;
        public TextMeshProUGUI tmp;

        public void Activate(float lifetime)
        {
            Destroy(gameObject, lifetime);
            canvasGroup.alpha = 1;
            canvasGroup.DOFade(0, lifetime).SetDelay(1f).OnComplete(() => { Destroy(gameObject); });
        }
    }
}