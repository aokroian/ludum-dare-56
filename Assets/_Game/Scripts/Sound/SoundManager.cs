using System;
using Player.Events;
using UnityEngine;
using Zenject;

namespace Sound
{
    public class SoundManager : MonoBehaviour
    {
        private SignalBus _signalBus;
        private SoundsConfig _soundsConfig;

        [Inject]
        private void Initialize(SignalBus signalBus, SoundsConfig soundsConfig)
        {
            _signalBus = signalBus;
            _soundsConfig = soundsConfig;
        }

        private void Awake()
        {
            SubscribeToSignals();
        }

        private void OnDestroy()
        {
            UnsubscribeFromSignals();
        }


        private void SubscribeToSignals()
        {
            _signalBus.Subscribe<PlayerStepEvent>(OnPlayerStep);
        }

        private void OnPlayerStep(PlayerStepEvent ev)
        {
            var clip = _soundsConfig.playerStepSounds[
                UnityEngine.Random.Range(0, _soundsConfig.playerStepSounds.Length)];
            if (clip)
            {
                AudioSource.PlayClipAtPoint(clip, ev.Pos);
            }
        }

        private void UnsubscribeFromSignals()
        {
        }
    }
}