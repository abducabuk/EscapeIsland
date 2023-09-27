
using CbkSDK.State;

namespace CbkSDK.UI.Scripts
{
    public class LevelFailedPanelMediator : BaseStateUIPanelMediator
    {
        protected override string EnterEventName => GameStateEvents.ON_LEVELFAILED_ENTER;
        protected override string ExitEventName => GameStateEvents.ON_LEVELFAILED_EXIT;
        protected override bool InState => GetService<IGameStateService>().IsLevelFailed;
    }
}