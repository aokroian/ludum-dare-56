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
        private static Camera Camera => Camera.main;
        private Vector3 PosInFrontOfCamera => Camera.transform.position + Camera.transform.forward;

        [Inject]
        private void Initialize(SignalBus signalBus, SoundsConfig soundsConfig)
        {
            _signalBus = signalBus;
            _soundsConfig = soundsConfig;
            SubscribeToSignals();
        }

        private void FixedUpdate()
        {
            if (!Camera)
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

            _signalBus.Subscribe<EnemyGotHitEvent>(OnEnemyDied);
            _signalBus.Subscribe<AttackPlayerEvent>(OnEnemyAttackedPlayer);
            _signalBus.Subscribe<EnemyRepositionEvent>(OnEnemyRepositioned);
        }

        private void OnEnemyAttackedPlayer()
        {
            var prepareToAttackClip = _soundsConfig.enemyPrepareToAttackSounds.UnityRandom();
            var length = prepareToAttackClip.length;
            Extensions.CustomPlayClipAtPoint(prepareToAttackClip, PosInFrontOfCamera, mainMixerGroup);
            Observable.Timer(TimeSpan.FromSeconds(length))
                .ObserveOnCurrentSynchronizationContext()
                .Subscribe(
                    _ =>
                    {
                        var clip = _soundsConfig.enemyAttackSounds.UnityRandom();
                        Extensions.CustomPlayClipAtPoint(clip, PosInFrontOfCamera, mainMixerGroup);
                    });
        }

        private void OnNightStarted()
        {
            var clip = _soundsConfig.newNightSounds.UnityRandom();
            var length = clip.length;
            Extensions.CustomPlayClipAtPoint(clip, PosInFrontOfCamera, mainMixerGroup);
            Observable.Timer(TimeSpan.FromSeconds(length * .5f))
                .ObserveOnCurrentSynchronizationContext()
                .Subscribe(
                    _ =>
                    {
                        var getUpFromBedClip = _soundsConfig.getUpFromBedSounds.UnityRandom();
                        Extensions.CustomPlayClipAtPoint(getUpFromBedClip, PosInFrontOfCamera, mainMixerGroup);
                    });
        }

        private void OnEnemyRepositioned(EnemyRepositionEvent ev)
        {
            var clip = _soundsConfig.enemyRepositionedSounds.UnityRandom();
            Observable.Timer(TimeSpan.FromSeconds(.5f))
                .ObserveOnCurrentSynchronizationContext()
                .Subscribe(_ => { Extensions.CustomPlayClipAtPoint(clip, ev.Pos, mainMixerGroup); });
        }

        private void OnEnemyDied(EnemyGotHitEvent ev)
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
            var cameraTarget = Camera.transform.position + Camera.transform.forward * 15f;
            cameraAnim.PlayAnim(Camera, cameraTarget, clip.length * .7f);
        }

        private void OnGameSceneLoaded()
        {
            var clip = _soundsConfig.gameMusicClips.UnityRandom();
            FadeToMusicClip(clip);
        }

        private void OnMenuSceneLoaded()
        {
            var clip = _soundsConfig.menuMusicClips.UnityRandom();
            FadeToMusicClip(clip);
        }

        private void FadeToMusicClip(AudioClip clip)
        {
            if (_activeMusicSource) _activeMusicSource.DOFade(0f, .7f);
            var nextAudioSource = _activeMusicSource == musicSource1
                ? musicSource2
                : musicSource1;
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