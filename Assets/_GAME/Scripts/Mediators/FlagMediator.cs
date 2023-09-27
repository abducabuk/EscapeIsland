using _GAME.Scripts.Events;
using _GAME.Scripts.Other;
using _GAME.Scripts.Services;
using CbkSDK.Core.Event.Interface;
using CbkSDK.Core.View;
using CbkSDK.Haptic;
using CbkSDK.Util.General;
using UnityEngine;

namespace _GAME.Scripts.Mediators
{
    public class FlagMediator : BaseMediator
    {
        [SerializeField] private AreaMediator _areaMediator;
        [SerializeField] private ColorTypeObject _flagColorObject;
        [SerializeField] private LocalMove _localMove;
        [SerializeField] private ParticleSystem _confetti;
        private IAreaCrowdService _areaCrowdService;
        private IHapticService _hapticService;
        private void OnEnable()
        {
            _areaCrowdService = GetService<IAreaCrowdService>();
            _hapticService = GetService<IHapticService>();
            ReverseIfNeeded();
            Subscribe(GameEvents.ON_AREA_CROWD_SERVICE_MOVE,OnAreaCrowdMove);

        }

        private void ReverseIfNeeded()
        {
            var localScale = transform.localScale;
            localScale.x *= -Mathf.Sign(transform.position.x);
            transform.localScale = localScale;
        }

        private void OnDisable()
        {
            Unsubscribe(GameEvents.ON_AREA_CROWD_SERVICE_MOVE,OnAreaCrowdMove);

        }

        private void OnAreaCrowdMove(IEvent e)
        {
            if (e is AreasCrowdMoveEvent areasCrowdMoveEvent 
                && areasCrowdMoveEvent.second==_areaMediator.Index 
                && _areaCrowdService.IsAreaSolved(_areaMediator.Index))
            {
                this.StartWaitForSecondCoroutine(1.75f, () =>
                {
                    _flagColorObject.SetCurrent(_areaCrowdService.AreaType(_areaMediator.Index));
                    _localMove.SetDestinationY(0);
                    _confetti.Play(true);
                    _hapticService.SuccessHaptic();
                });
            }
        }
    }
}