using System;
using DG.Tweening;
using UnityEngine;

namespace Tutorial
{
    public class PulseEffect : MonoBehaviour
    {
        public bool activateOnEnable;

        private Vector3 _initialScale;
        private bool IsActive => _tween != null && _tween.IsPlaying();
        private Tween _tween;

        private void Awake()
        {
            _initialScale = transform.localScale;
        }

        private void OnEnable()
        {
            if (activateOnEnable) Toggle(true);
        }

        private void OnDisable()
        {
            if (IsActive)
            {
                Toggle(false);
            }
        }

        public void Toggle(bool active)
        {
            if (active && IsActive)
                return;
            if (active)
            {
                _tween = transform.DOScale(_initialScale * 1.1f, 0.5f).SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
            }
            else if (_tween != null)
            {
                _tween.Kill();
                transform.localScale = _initialScale;
                _tween = null;
            }
        }
    }
}