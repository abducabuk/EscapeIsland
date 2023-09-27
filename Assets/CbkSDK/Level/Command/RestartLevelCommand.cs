using CbkSDK.Core.Command;
using CbkSDK.Core.Event.Interface;
using CbkSDK.State;

namespace CbkSDK.Level.Command
{
    public class RestartLevelCommand : BaseCommand
    {
        protected override void OnExecute(IEvent e = null)
        {
            GetService<ILevelService>().RestartLevel();
            GetService<IGameStateService>().EnterMainMenu();
        }
    }
}