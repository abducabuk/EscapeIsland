using CbkSDK.Core.View;
using CbkSDK.Haptic;
using UnityEngine;
using UnityEngine.UI;

namespace CbkSDK.UI.Scripts
{
    public class HapticButtonMediator : BaseMediator
    {
        [SerializeField] private Image hapticOn;
        [SerializeField] private Image hapticOff;

        private IHapticService _hapticService;
        private void Awake()
        {
            _hapticService = GetService<IHapticService>();
        }

        private void Update()
        {
            var isActive = _hapticService.IsActive;
            if (isActive != hapticOn.enabled) hapticOn.enabled = isActive;
            if (isActive == hapticOff.enabled) hapticOff.enabled = !isActive;

        }
    }
}
