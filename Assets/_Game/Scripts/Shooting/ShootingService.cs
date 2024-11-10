using System;
using GameLoop.Events;
using Shooting.Events;
using UnityEngine;
using Zenject;

namespace Shooting
{
    public class ShootingService
    {
        [Inject]
        private SignalBus _signalBus;

        [Inject]
        private Config _config;

        private float _nextShotTime;

        public int Ammo => _isInfiniteAmmo ? int.MaxValue : _ammo;

        private int _ammo;
        private bool _isInfiniteAmmo;

        [Inject]
        private void Initialize(SignalBus signalBus, Config config)
        {
            _signalBus = signalBus;
            _config = config;
            _ammo = _config.startAmmo;
            _signalBus.Subscribe<NightStartedEvent>(OnNightStarted);
        }

        public void SetInfiniteAmmo(bool isInfinite) => _isInfiniteAmmo = isInfinite;

        public bool TryShoot()
        {
            if (Ammo <= 0)
            {
                Debug.Log($"No Ammo");
                _signalBus.Fire<ShootingNoAmmoEvent>();
                return false;
            }

            if (_nextShotTime > Time.time)
            {
                Debug.Log($"Wait...");
                return false;
            }

            _nextShotTime = Time.time + _config.shotDelay;

            _ammo--;
            _signalBus.Fire<ShootingEvent>();
            return true;
        }

        private void OnNightStarted(NightStartedEvent e)
        {
            _ammo = _config.startAmmo;
        }

        [Serializable]
        public class Config
        {
            public int startAmmo;
            public float shotDelay;
        }
    }
}