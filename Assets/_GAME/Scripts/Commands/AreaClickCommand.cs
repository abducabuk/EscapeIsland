using _GAME.Scripts.Events;
using _GAME.Scripts.Services;
using CbkSDK.Core.Command;
using CbkSDK.Core.Event.Attribute;
using CbkSDK.Core.Event.Interface;
using CbkSDK.Haptic;

namespace _GAME.Scripts.Commands
{
    [BindEvent(GameEvents.ON_AREA_CLICK)]
    public class AreaClickCommand : BaseCommand
    {
        protected override void OnExecute(IEvent e = null)
        {
            if (e is AreaClickEvent areaClick)
            {
                var areaService = GetService<IAreaService>();
                var areaCrowdService = GetService<IAreaCrowdService>();
                var hapticService = GetService<IHapticService>();
                var clickedIndex = areaClick.index;
                areaCrowdService.IsAreaFilled(clickedIndex);
                var lastClicked = areaService.LastClicked;
                
                if (lastClicked == -1)
                {
                    if (areaCrowdService.IsAreaEmpty(clickedIndex) || areaCrowdService.IsAreaSolved(clickedIndex))
                    {
                        Fire(GameEvents.ON_AREAS_DESELECTED);
                    }
                    else
                    {
                        lastClicked = clickedIndex;
                        hapticService.HeavyHaptic();
                    }
                }
                else if (lastClicked == clickedIndex)
                {
                    Fire(GameEvents.ON_AREAS_DESELECTED);
                    lastClicked = -1;
                }
                else if (areaCrowdService.IsAreaFilled(clickedIndex))
                {
                    Fire(GameEvents.ON_AREAS_DESELECTED);
                    lastClicked = -1;
                }
                else
                {
                    Fire(GameEvents.ON_AREAS_SELECTED,new AreasSelectEvent(){first =lastClicked,second = clickedIndex});
                    lastClicked = -1;
                }

                areaService.LastClicked = lastClicked;


            }
        }
    }
}