using System;
using System.Collections.Generic;
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

        public int Matches => _isInfiniteMatches ? int.MaxValue : _matchesCount;

        private SignalBus _signalBus;
        private float _nextLightTime;
        private Action _cancelCallback;

        private List<IDisposable> _disposables = new();

        private float Duration => _isOverwrittenDuration ? _overwrittenDuration : _config.duration;

        private int _matchesCount;
        private bool _isInfiniteMatches;
        private float _overwrittenDuration;
        private bool _isOverwrittenDuration;


        [Inject]
        private void Initialize(Config config, SignalBus signalBus)
        {
            _config = config;
            _signalBus = signalBus;
            _matchesCount = _config.startMatches;
            _signalBus.Subscribe<NightStartedEvent>(OnNightStarted);
        }

        public void SetInfiniteMatches(bool isInfinite) => _isInfiniteMatches = isInfinite;

        public void SetOverwrittenDuration(bool isOn, float value = 0)
        {
            _isOverwrittenDuration = isOn;
            _overwrittenDuration = value;
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

            _nextLightTime = Time.time + Duration + _config.delay;

            _matchesCount--;
            _signalBus.Fire(new MatchLitEvent());
            _cancelCallback = cancelCallback;

            Observable.Timer(TimeSpan.FromSeconds(Duration))
                .ObserveOn(UnityFrameProvider.Update)
                .Subscribe(_ =>
                {
                    _signalBus.Fire(new MatchWentOutEvent());
                    _cancelCallback = null;
                }).AddTo(_disposables);

            return Duration;
        }

        private void OnNightStarted(NightStartedEvent e)
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }

            _disposables.Clear();
            _matchesCount = _config.startMatches;
            _nextLightTime = 0;
            _cancelCallback?.Invoke();
        }

        public bool IsLit()
        {
            return _nextLightTime > Time.time;
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