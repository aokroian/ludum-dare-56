using System;
using UnityEngine;

namespace Matchstick
{
    public class Matchstick : MonoBehaviour
    {
        [SerializeField] private ParticleSystem flame;
        [SerializeField] private Transform flameStartPoint;
        [SerializeField] private Transform flameEndPoint;
        
        private float _duration;
        private float _burnLeft;
        private Action _onFinished;


        public void Light(float duration, Action onFinished)
        {
            _onFinished = onFinished;
            _duration = duration;
            _burnLeft = duration;
            flame.time = 0f;
            flame.Play();
        }

        private void Update()
        {
            if (_burnLeft <= 0f)
            {
                flame.Stop();
                _onFinished?.Invoke();
                return;
            }

            if (_burnLeft < 0.3)
            {
                flame.transform.localScale = Vector3.Lerp(flameStartPoint.localScale, flameEndPoint.localScale,
                    1 - _burnLeft);
            }

            flame.transform.position = Vector3.Lerp(flameStartPoint.position, flameEndPoint.position,
                1 - _burnLeft / _duration);
            _burnLeft -= Time.deltaTime;
        }
    }
}