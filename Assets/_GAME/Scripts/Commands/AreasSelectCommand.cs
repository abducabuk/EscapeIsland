using _GAME.Scripts.Events;
using _GAME.Scripts.Services;
using CbkSDK.Core.Command;
using CbkSDK.Core.Event.Attribute;
using CbkSDK.Core.Event.Interface;
using CbkSDK.Haptic;

namespace _GAME.Scripts.Commands
{
    [BindEvent(GameEvents.ON_AREAS_SELECTED)]
    public class AreasSelectCommand : BaseCommand
    {
        protected override void OnExecute(IEvent e = null)
        {
            if (e is AreasSelectEvent areasSelect)
            {
                var hapticService = GetService<IHapticService>();
                var moveCount = GetService<IAreaCrowdService>().Move(areasSelect.first,areasSelect.second);
                if (moveCount > 0)
                {
                    Fire(GameEvents.ON_AREA_CROWD_SERVICE_MOVE,new AreasCrowdMoveEvent(){first = areasSelect.first,second = areasSelect.second,moveCount = moveCount});
                    hapticService.HeavyHaptic();
                }
            }
        }
    }
}