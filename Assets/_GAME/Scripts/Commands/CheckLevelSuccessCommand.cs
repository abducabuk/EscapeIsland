using _GAME.Scripts.Events;
using _GAME.Scripts.Services;
using CbkSDK.Core.Async;
using CbkSDK.Core.Command;
using CbkSDK.Core.Event.Attribute;
using CbkSDK.Core.Event.Interface;
using CbkSDK.Level;
using CbkSDK.Level.Command;

namespace _GAME.Scripts.Commands
{
    [BindEvent(GameEvents.ON_AREA_CROWD_SERVICE_MOVE)]
    public class CheckLevelSuccessCommand : BaseCommand
    {
        protected override void OnExecute(IEvent e = null)
        {
            if (e is AreasCrowdMoveEvent areasSelect)
            {
                var areaCrowdService = GetService<IAreaCrowdService>();
                var asyncService = GetService<IAsyncService>();
                if (areaCrowdService.IsAllAreaSolved())
                {
                    asyncService.WaitForSecond(2f, SuccessLevel);
                }
            }
        }

        private void SuccessLevel()
        {
            new SuccessLevelCommand().Execute();
        }
    }
}