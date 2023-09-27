
using CbkSDK.State;

namespace CbkSDK.UI.Scripts
{
    public class MainMenuPanelMediator : BaseStateUIPanelMediator
    {
        protected override string EnterEventName => GameStateEvents.ON_MAINMENU_ENTER;
        protected override string ExitEventName => GameStateEvents.ON_MAINMENU_EXIT;
        protected override bool InState => GetService<IGameStateService>().IsMainMenu;
    }
}