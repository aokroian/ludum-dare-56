﻿using System;
using DG.Tweening;
using GameLoop.Events;
using Matchstick.Events;
using R3;
using UnityEngine;
using Zenject;

namespace Matchstick
{
    public class MatchService
    {
        private Config _config;

        public int Matches { get; private set; }

        private SignalBus _signalBus;
        private float _nextLightTime;
        private Action _cancelCallback;
        
        private DisposableBag _disposables;


        [Inject]
        private void Initialize(Config config, SignalBus signalBus)
        {
            _config = config;
            _signalBus = signalBus;

            Matches = _config.startMatches;
            
            _signalBus.Subscribe<NightStartedEvent>(OnNightStarted);
        }

        public float TryLight(Action cancelCallback)
        {
            if (Matches <= 0)
            {
                return 0;
            }

            if (_nextLightTime > Time.time)
            {
                return 0;
            }

            _nextLightTime = Time.time + _config.duration + _config.delay;

            Matches--;
            _signalBus.Fire(new MatchLitEvent());
            _cancelCallback = cancelCallback;

            Observable.Timer(TimeSpan.FromSeconds(_config.duration))
                .ObserveOnCurrentSynchronizationContext()
                .Subscribe(_ =>
                {
                    _signalBus.Fire(new MatchWentOutEvent());
                    _cancelCallback = null;
                }).AddTo(ref _disposables);

            return _config.duration;
        }
        
        private void OnNightStarted(NightStartedEvent e)
        {
            _disposables.Dispose();
            Matches = _config.startMatches;
            _nextLightTime = 0;
            _cancelCallback?.Invoke();
        }
        
        [Serializable]
        public class Config
        {
            public int startMatches;
            public float duration;
            public float delay;
        }
    }
}