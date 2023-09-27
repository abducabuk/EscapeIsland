using System.Collections.Generic;
using System.Linq;
using _GAME.Scripts.Events;
using _GAME.Scripts.Services;
using CbkSDK.Core.Event.Interface;
using CbkSDK.Core.View;
using CbkSDK.Pooling;
using UnityEngine;
using UnityEngine.Rendering;

namespace _GAME.Scripts.Mediators
{
    public class AreaCrowdMediator : BaseMediator
    {
        [SerializeField] private SerializedDictionary<int, List<StickmanMediator>> _stickmans=new SerializedDictionary<int, List<StickmanMediator>>();
        private IAreaCrowdService _areaCrowdService;
        private IPoolService _poolService;
        
        void OnEnable()
        {
            _areaCrowdService = GetService<IAreaCrowdService>();
            _poolService = GetService<IPoolService>();
            Subscribe(GameEvents.ON_AREA_CROWD_SERVICE_FILLED,OnAreaCrowdServiceFilled);
            Subscribe(GameEvents.ON_AREA_CROWD_SERVICE_MOVE,OnAreaCrowdServiceMove);
        }
        
        void OnDisable()
        {
            Unsubscribe(GameEvents.ON_AREA_CROWD_SERVICE_FILLED,OnAreaCrowdServiceFilled);
            Unsubscribe(GameEvents.ON_AREA_CROWD_SERVICE_MOVE,OnAreaCrowdServiceMove);
            _poolService.DeActivateAllObjectFromPool("stickman");
        }

        private void OnAreaCrowdServiceMove(IEvent e)
        {
            if (e is AreasCrowdMoveEvent areasCrowdMoveEvent)
            {
                var stickmanPerSquad = _areaCrowdService.StickmanPerSquad;

                for (var i = 0; i < areasCrowdMoveEvent.moveCount; i++)
                {
                    for (var s = 0; s < stickmanPerSquad; s++)
                    {
                        var firstList = _stickmans[areasCrowdMoveEvent.first];
                        var secondList = _stickmans[areasCrowdMoveEvent.second];
                        var movedStickman = firstList.Last();
                        firstList.RemoveAt(firstList.Count-1);
                        secondList.Add(movedStickman);
                        var moveIndex = i * areasCrowdMoveEvent.moveCount + s;
                        movedStickman.SetDestination(areasCrowdMoveEvent.second,(secondList.Count - 1)/stickmanPerSquad,(s+2)%stickmanPerSquad,moveIndex*0.1f);
                    }
                }

            }
        }


        private void OnAreaCrowdServiceFilled(IEvent obj)
        {
            CreateStickmans();
        }

        private void CreateStickmans()
        {
            var areaCrowds = _areaCrowdService.AreaCrowds;
            foreach (var index in areaCrowds.Keys)
            {
                if (!_stickmans.ContainsKey(index))
                {
                    _stickmans.Add(index,new List<StickmanMediator>());
                }
                var stickmanPerSquad = _areaCrowdService.StickmanPerSquad;
                var crowdTypes = areaCrowds[index];
                for (var i = 0; i < crowdTypes.Count; i++)
                {
                    for (var s = 0; s < stickmanPerSquad; s++)
                    {
                        var crowdType = crowdTypes[i];
                        var stickman = _poolService.Instantiate("stickman",Vector3.zero,Quaternion.identity).GetComponent<StickmanMediator>();
                        stickman.SetType(crowdType);
                        stickman.SetDestinationImmediate(index,i,s);
                        _stickmans[index].Add(stickman);
                    }
                }
            }
        }
        
    }
}
