using _GAME.Scripts.Mediators;
using CbkSDK.Util.General;
using UnityEngine;

namespace _GAME.Scripts.Other
{
    public class AreaUpDown : BaseObjectUpdater<bool>
    {
        public const float UP_HEIGHT = 1.5f;
        [SerializeField] private AreaMediator _areaMediator;
        [SerializeField] private BaseMove _move;

        protected override bool Value => _areaMediator.IsSelected;
        protected override void OnValueUpdate()
        {
            UpdatePosition();
        }
        private void UpdatePosition()
        {
            if (value)
            {
                _move.SetDestinationY(UP_HEIGHT);
            }
            else
            {
                _move.ResetDestination();
            }
        }
    }
}