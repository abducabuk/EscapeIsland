using CbkSDK.Core.Command;
using CbkSDK.Core.Event.Interface;
using CbkSDK.State;

namespace CbkSDK.Level.Command
{
    public class PreviousLevelCommand : BaseCommand
    {
        protected override void OnExecute(IEvent e = null)
        {
            var levelService = GetService<ILevelService>();
            levelService.Level--;
            levelService.RestartLevel();
            GetService<IGameStateService>().EnterMainMenu();
        }
    }
}