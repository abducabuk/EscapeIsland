using System;
using CandyCoded.HapticFeedback;
using CbkSDK.Core.ServiceLocator;
using CbkSDK.Core.ServiceLocator.Attribute;
using CbkSDK.Core.ServiceLocator.Interface;
using CbkSDK.PlayerPrefs;
using UnityEngine;

namespace CbkSDK.Haptic
{
    public interface IHapticService : IService
    {
        public bool IsActive { get; }
        public void SuccessHaptic();
        public void FailedHaptic();
        public void LightHaptic();

        public void MediumHaptic();

        public void HeavyHaptic();

        public void SetActive(bool state);

        public void HapticToggle();

    }
    [Service(false)]
    public class HapticService : BaseService , IHapticService
    {
        private const string PLAYER_PREFS_KEY = "haptic";
        private const float HAPTIC_INTERVAL = 0.15f;
    
        private bool _isActive = true;
        private float _lastHapticTime;

        public bool IsActive => _isActive;
        private IPlayerPrefsService _playerPrefsService;
        public void Initialize()
        {
            _playerPrefsService = GetService<IPlayerPrefsService>();
            if (!_playerPrefsService.Load(PLAYER_PREFS_KEY, out _isActive))
            {
                _isActive = true;
            }
        }

        private void TryHaptic(Action action)
        {
            if (_isActive && Time.time - _lastHapticTime > HAPTIC_INTERVAL )
            {
                _lastHapticTime = Time.time;
                try
                {
                    action?.Invoke();
                }
                catch
                {
                    LogError("Haptic Error");
                }
            }
        }
        public void SuccessHaptic()=>TryHaptic(()=>HapticFeedback.HeavyFeedback());
        public void FailedHaptic()=>TryHaptic(()=>HapticFeedback.HeavyFeedback());
        public void LightHaptic()=>TryHaptic(()=>HapticFeedback.LightFeedback());
        public void MediumHaptic()=>TryHaptic(()=>HapticFeedback.MediumFeedback());
        public void HeavyHaptic()=>TryHaptic(()=>HapticFeedback.HeavyFeedback());
        public void SetActive(bool state)
        {
            _isActive = state;
            _playerPrefsService.Save(PLAYER_PREFS_KEY, _isActive);
            if (_isActive) HeavyHaptic();
        }
    
        public void HapticToggle()
        {
            SetActive(!_isActive);
        }
    }
}