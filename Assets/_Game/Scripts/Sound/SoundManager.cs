using System;
using _GameTemplate.Scripts.Common;
using DG.Tweening;
using Matchstick.Events;
using Player.Events;
using R3;
using Shooting.Events;
using UnityEngine;
using Zenject;

namespace Sound
{
    public class SoundManager : SingletonGlobal<SoundManager>
    {
        [SerializeField] private AudioSource musicSource1;
        [SerializeField] private AudioSource musicSource2;

        private SignalBus _signalBus;
        private SoundsConfig _soundsConfig;
        private AudioSource _currentMusicSource;
        private Camera _camera;
        private Vector3 PosInFrontOfCamera => _camera.transform.position + _camera.transform.forward;

        [Inject]
        private void Initialize(SignalBus signalBus, SoundsConfig soundsConfig)
        {
            _signalBus = signalBus;
            _soundsConfig = soundsConfig;
        }

        private void Start()
        {
            _camera = Camera.main;
            SubscribeToSignals();

            // todo: delete after implementing scene management signals
            OnGameMusicRequested();
        }

        private void FixedUpdate()
        {
            musicSource1.gameObject.transform.position = PosInFrontOfCamera;
            musicSource2.gameObject.transform.position = PosInFrontOfCamera;
        }

        private void OnDestroy()
        {
            UnsubscribeFromSignals();
        }

        private void SubscribeToSignals()
        {
            _signalBus.Subscribe<PlayerStepEvent>(OnPlayerStep);
            _signalBus.Subscribe<MatchLitEvent>(OnMatchstickLit);
            _signalBus.Subscribe<ShootingEvent>(OnShoot);
            _signalBus.Subscribe<ShootingNoAmmoEvent>(OnShootNoAmmo);
        }

        private void OnShoot()
        {
            var clip = _soundsConfig.shootSounds.UnityRandom();
            var clipLength = clip.length;
            Invoke(nameof(OnCasings), clipLength * .5f);
            AudioSource.PlayClipAtPoint(clip, PosInFrontOfCamera);
        }

        private void OnCasings()
        {
            var casings = _soundsConfig.bulletCasingsSounds.UnityRandom();
            AudioSource.PlayClipAtPoint(casings, PosInFrontOfCamera);
        }

        private void OnShootNoAmmo()
        {
            var clip = _soundsConfig.shootNoAmmoSounds.UnityRandom();
            AudioSource.PlayClipAtPoint(clip, PosInFrontOfCamera);
        }

        private void UnsubscribeFromSignals()
        {
        }

        private void OnPlayerStep(PlayerStepEvent ev)
        {
            var clip = _soundsConfig.playerStepSounds.UnityRandom();
            if (clip)
            {
                AudioSource.PlayClipAtPoint(clip, ev.Pos);
            }
        }

        private void OnMatchstickLit()
        {
            var clip = _soundsConfig.matchStickSounds.UnityRandom();
            AudioSource.PlayClipAtPoint(clip, PosInFrontOfCamera);
        }

        private void OnGameMusicRequested()
        {
            var clip = _soundsConfig.gameMusicClips.UnityRandom();
            FadeToMusicClip(clip);
        }

        private void FadeToMusicClip(AudioClip clip)
        {
            var targetSource = _currentMusicSource == musicSource1 ? musicSource2 : musicSource1;
            targetSource.clip = clip;
            targetSource.volume = 0f;
            targetSource.Play();
            targetSource.DOFade(1, 1f);
            _currentMusicSource.DOKill();
            _currentMusicSource.DOFade(0, 1f).OnComplete(() => _currentMusicSource.Stop());
            _currentMusicSource = targetSource;
        }
    }
}