using CbkSDK.Core.Command;
using CbkSDK.Core.Event.Interface;
using CbkSDK.Haptic;
using CbkSDK.State;

namespace CbkSDK.Level.Command
{
    public class SuccessLevelCommand : BaseCommand
    {
        protected override void OnExecute(IEvent e = null)
        {
            GetService<IGameStateService>().EnterLevelSuccess();
            GetService<ILevelService>().Success();
            GetService<IHapticService>().SuccessHaptic();
        }
    }
}