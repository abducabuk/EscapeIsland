using CbkSDK.Core.Command;
using CbkSDK.Core.Event.Interface;
using CbkSDK.Haptic;
using CbkSDK.State;

namespace CbkSDK.Level.Command
{
    public class FailLevelCommand : BaseCommand
    {
        protected override void OnExecute(IEvent e = null)
        {
            GetService<IGameStateService>().EnterLevelFailed();
            GetService<ILevelService>().Fail();
            GetService<IHapticService>().FailedHaptic();
        }
    }
}