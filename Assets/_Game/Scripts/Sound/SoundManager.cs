using System;
using Cutscenes;
using DG.Tweening;
using Enemy.Events;
using GameLoop.Events;
using Matchstick.Events;
using Player.Events;
using R3;
using Shooting.Events;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Sound
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioSource musicSource1;
        [SerializeField] private AudioSource musicSource2;
        [SerializeField] private AudioMixerGroup mainMixerGroup;

        private SignalBus _signalBus;
        private SoundsConfig _soundsConfig;
        private AudioSource _activeMusicSource;
        private Camera _camera;
        private Vector3 PosInFrontOfCamera
        {
            get
            {
#if UNITY_EDITOR
                if (!_camera) return Vector3.zero;
#endif
                return _camera.transform.position + _camera.transform.forward;
            }
        }

        [Inject]
        private void Initialize(SignalBus signalBus, SoundsConfig soundsConfig)
        {
            _signalBus = signalBus;
            _soundsConfig = soundsConfig;
            SubscribeToSignals();
        }

        private void FixedUpdate()
        {
            if (!_camera)
            {
                return;
            }

            musicSource1.gameObject.transform.position = PosInFrontOfCamera;
            musicSource2.gameObject.transform.position = PosInFrontOfCamera;
        }

        private void SubscribeToSignals()
        {
            _signalBus.Subscribe<NightStartedEvent>(OnNightStarted);

            _signalBus.Subscribe<PlayerStepEvent>(OnPlayerStep);
            _signalBus.Subscribe<MatchLitEvent>(OnMatchstickLit);
            _signalBus.Subscribe<ShootingEvent>(OnShoot);
            _signalBus.Subscribe<ShootingNoAmmoEvent>(OnShootNoAmmo);

            _signalBus.Subscribe<GameStartPressedEvent>(OnGameStartPressed);
            _signalBus.Subscribe<GameSceneLoadedEvent>(OnGameSceneLoaded);
            _signalBus.Subscribe<MenuSceneLoadedEvent>(OnMenuSceneLoaded);

            _signalBus.Subscribe<EnemyDiedEvent>(OnEnemyDied);
            _signalBus.Subscribe<AttackPlayerEvent>(OnEnemyAttackedPlayer);
            _signalBus.Subscribe<EnemyRepositionEvent>(OnEnemyRepositioned);
        }

        private void OnEnemyAttackedPlayer()
        {
            var clip = _soundsConfig.enemyAttackSounds.UnityRandom();
            Extensions.CustomPlayClipAtPoint(clip, PosInFrontOfCamera, mainMixerGroup);
        }

        private void OnNightStarted()
        {
            var clip = _soundsConfig.newNightSounds.UnityRandom();
            var length = clip.length;
            Extensions.CustomPlayClipAtPoint(clip, PosInFrontOfCamera, mainMixerGroup);
            Observable.Timer(TimeSpan.FromSeconds(length * .5f)).ObserveOnCurrentSynchronizationContext()
                .Subscribe(_ =>
                {
                    var getUpFromBedClip = _soundsConfig.getUpFromBedSounds.UnityRandom();
                    Extensions.CustomPlayClipAtPoint(getUpFromBedClip, PosInFrontOfCamera, mainMixerGroup);
                });
        }

        private void OnEnemyRepositioned(EnemyRepositionEvent ev)
        {
            var clip = _soundsConfig.enemyRepositionedSounds.UnityRandom();
            Observable.Timer(TimeSpan.FromSeconds(.5f)).ObserveOnCurrentSynchronizationContext()
                .Subscribe(_ => { Extensions.CustomPlayClipAtPoint(clip, ev.Pos, mainMixerGroup); });
        }

        private void OnEnemyDied(EnemyDiedEvent ev)
        {
            var clip = _soundsConfig.enemyDiedSounds.UnityRandom();
            Extensions.CustomPlayClipAtPoint(clip, ev.Pos, mainMixerGroup);
        }

        private void OnShoot()
        {
            var clip = _soundsConfig.shootSounds.UnityRandom();
            var clipLength = clip.length;
            Invoke(nameof(OnCasings), clipLength * .5f);
            Invoke(nameof(OnCock), clipLength);
            Extensions.CustomPlayClipAtPoint(clip, PosInFrontOfCamera, mainMixerGroup);
        }

        private void OnCasings()
        {
            var casings = _soundsConfig.bulletCasingsSounds.UnityRandom();
            Extensions.CustomPlayClipAtPoint(casings, PosInFrontOfCamera, mainMixerGroup);
        }

        private void OnCock()
        {
            var clip = _soundsConfig.cockSounds.UnityRandom();
            Extensions.CustomPlayClipAtPoint(clip, PosInFrontOfCamera, mainMixerGroup);
        }

        private void OnShootNoAmmo()
        {
            var clip = _soundsConfig.shootNoAmmoSounds.UnityRandom();
            Extensions.CustomPlayClipAtPoint(clip, PosInFrontOfCamera, mainMixerGroup);
        }

        private void OnPlayerStep(PlayerStepEvent ev)
        {
            var clip = _soundsConfig.playerStepSounds.UnityRandom();
            if (clip)
            {
                Extensions.CustomPlayClipAtPoint(clip, ev.Pos, mainMixerGroup);
            }
        }

        private void OnMatchstickLit()
        {
            var clip = _soundsConfig.matchStickSounds.UnityRandom();
            Extensions.CustomPlayClipAtPoint(clip, PosInFrontOfCamera, mainMixerGroup);
        }

        private void OnGameStartPressed()
        {
            FadeToMusicClip(null);
            var clip = _soundsConfig.gameStartPressedSound;
            Extensions.CustomPlayClipAtPoint(clip, PosInFrontOfCamera, mainMixerGroup);
            var cameraAnim = new GameStartCameraAnimation();
            var cameraTarget = _camera.transform.position + _camera.transform.forward * 15f;
            cameraAnim.PlayAnim(_camera, cameraTarget, clip.length * .7f);
        }

        private void OnGameSceneLoaded()
        {
            _camera = Camera.main;
            var clip = _soundsConfig.gameMusicClips.UnityRandom();
            FadeToMusicClip(clip);
        }

        private void OnMenuSceneLoaded()
        {
            _camera = Camera.main;
            var clip = _soundsConfig.menuMusicClips.UnityRandom();
            FadeToMusicClip(clip);
        }

        private void FadeToMusicClip(AudioClip clip)
        {
            if (_activeMusicSource) _activeMusicSource.DOFade(0f, .7f);
            var nextAudioSource = _activeMusicSource == musicSource1 ? musicSource2 : musicSource1;
            if (clip)
            {
                nextAudioSource.clip = clip;
                nextAudioSource.Play();
                nextAudioSource.DOFade(1f, .7f);
            }

            _activeMusicSource = nextAudioSource;
        }
    }
}