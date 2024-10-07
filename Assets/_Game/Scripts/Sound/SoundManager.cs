using Cutscenes;
using DG.Tweening;
using GameLoop.Events;
using Matchstick.Events;
using Player.Events;
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
        private Vector3 PosInFrontOfCamera => _camera.transform.position + _camera.transform.forward;

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

            _signalBus.Subscribe<GameStartPressedEvent>(OnGameStartPressed);
            _signalBus.Subscribe<GameSceneLoadedEvent>(OnGameSceneLoaded);
            _signalBus.Subscribe<MenuSceneLoadedEvent>(OnMenuSceneLoaded);
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

        private void UnsubscribeFromSignals()
        {
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