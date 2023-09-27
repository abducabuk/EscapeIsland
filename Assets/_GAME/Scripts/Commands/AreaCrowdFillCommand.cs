using System.Collections.Generic;
using _GAME.Scripts.Events;
using _GAME.Scripts.Services;
using CbkSDK.Core.Command;
using CbkSDK.Core.Event.Attribute;
using CbkSDK.Core.Event.Interface;
using CbkSDK.Level;

namespace _GAME.Scripts.Commands
{
    [BindEvent(LevelEvents.ON_LEVEL_INIT)]
    public class AreaCrowdFillCommand : BaseCommand
    {
        protected override void OnExecute(IEvent e = null)
        {
            var areaCrowdFillService = GetService<IAreaCrowdFillService>();
            var levelService = GetService<ILevelService>();
            areaCrowdFillService.Fill(levelService.Level-1);
            Fire(GameEvents.ON_AREA_CROWD_SERVICE_FILLED);
        }
    }
}