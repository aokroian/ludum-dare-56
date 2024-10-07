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
        
        public int Ammo { get; private set; }
        
        [Inject]
        private void Initialize(SignalBus signalBus, Config config)
        {
            _signalBus = signalBus;
            _config = config;
            
            Ammo = _config.startAmmo;
            
            _signalBus.Subscribe<NightStartedEvent>(OnNightStarted);
        }

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

            Ammo--;
            
            _signalBus.Fire<ShootingEvent>();
            return true;
        }
        
        private void OnNightStarted(NightStartedEvent e)
        {
            Ammo = _config.startAmmo;
        }

        [Serializable]
        public class Config
        {
            public int startAmmo;
            public float shotDelay;
        }
    }
}