
using CbkSDK.State;

namespace CbkSDK.UI.Scripts
{
    public class LevelSuccessPanelMediator : BaseStateUIPanelMediator
    {
        protected override string EnterEventName => GameStateEvents.ON_LEVELSUCCESS_ENTER;
        protected override string ExitEventName => GameStateEvents.ON_LEVELSUCCESS_EXIT;
        protected override bool InState => GetService<IGameStateService>().IsLevelSuccess;
    }
}