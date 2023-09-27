using CbkSDK.Core.View;
using CbkSDK.Haptic;
using CbkSDK.Level;
using CbkSDK.Level.Command;
using CbkSDK.State;

namespace CbkSDK.UI.Scripts
{
    public class UIButtonMediator : BaseMediator
    {
        private IGameStateService _gameStateService;
        private ILevelService _levelService;
        private IHapticService _hapticService;

        private void Awake()
        {
            _gameStateService = GetService<IGameStateService>();
            _levelService = GetService<ILevelService>();
            _hapticService = GetService<IHapticService>();
        }

        public void PlayButton()
        {
            _gameStateService.EnterGameplay();
        }
        
        public void NextLevelButton()
        {
            new RestartLevelCommand().Execute();
        }
        
        public void PreLevelButton()
        {
            new PreviousLevelCommand().Execute();

        }
        public void RestartLevelButton()
        {
            new RestartLevelCommand().Execute();
        }

        public void HapticToggleButton()
        {
            _hapticService.HapticToggle();
        }
    
        public void AudioToggleButton()
        {
            //TODO 
        }
    }
}