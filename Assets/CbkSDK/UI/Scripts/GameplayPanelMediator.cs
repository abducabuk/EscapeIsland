
using CbkSDK.State;

namespace CbkSDK.UI.Scripts
{
    public class GameplayPanelMediator : BaseStateUIPanelMediator
    {
        protected override string EnterEventName => GameStateEvents.ON_GAMEPLAY_ENTER;
        protected override string ExitEventName => GameStateEvents.ON_GAMEPLAY_EXIT;
        protected override bool InState => GetService<IGameStateService>().IsGameplay;
    }
}