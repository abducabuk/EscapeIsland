using _GAME.Scripts.Events;
using _GAME.Scripts.Services;
using _GAME.Scripts.Util;
using CbkSDK.Core.Event.Interface;
using CbkSDK.Core.View;
using CbkSDK.Util.General;
using UnityEngine;

namespace _GAME.Scripts.Mediators
{
    public class LineMediator : BaseMediator
    {
        [SerializeField] private MultipleLineRenderer _multipleLineRenderer;
        private IAreaService _areaService;
        private IAreaPositionService _areaPositionService;
        private IAreaCrowdService _areaCrowdService;

        private void OnEnable()
        {
            _areaService = GetService<IAreaService>();
            _areaPositionService = GetService<IAreaPositionService>();
            _areaCrowdService = GetService<IAreaCrowdService>();
            Subscribe(GameEvents.ON_AREA_CROWD_SERVICE_MOVE,OnAreaCrowdMove);
        }
        private void OnDisable()
        {
            Unsubscribe(GameEvents.ON_AREA_CROWD_SERVICE_MOVE,OnAreaCrowdMove);
        }

        private void OnAreaCrowdMove(IEvent e)
        {
            if (e is AreasCrowdMoveEvent areasCrowdMove)
            {
                var firstArea = _areaService.Get(areasCrowdMove.first).GetComponent<AreaMediator>();
                var secondArea = _areaService.Get(areasCrowdMove.second).GetComponent<AreaMediator>();
                var firstPos = firstArea.transform.position;
                var secondPos = secondArea.transform.position;
                var positions = _areaPositionService.GetPath(firstPos, secondPos);
                _multipleLineRenderer.DrawLine(positions);
                var delay = _areaPositionService.PathDistance(positions) / _areaCrowdService.StickmanSpeed;
                this.StartWaitForSecondCoroutine(delay+areasCrowdMove.moveCount*_areaCrowdService.StickmanPerSquad * 0.1f, () =>
                {
                    _multipleLineRenderer.Count--;
                });
            }
        }
    }
}
