using _GAME.Scripts.Events;
using _GAME.Scripts.Services;
using CbkSDK.Core.Event.Interface;
using CbkSDK.Core.View;
using UnityEngine;

namespace _GAME.Scripts.Mediators
{
    public class AreaMediator : BaseMediator
    {
        [SerializeField] private int _index;
        private bool _isSelected = false;

        public int Index => _index;
        public bool IsSelected => _isSelected;
        void OnEnable()
        {
            GetService<IAreaService>().RegisterArea(_index,gameObject);
            Subscribe(GameEvents.ON_AREAS_SELECTED,OnAreaSelected);
            Subscribe(GameEvents.ON_AREAS_DESELECTED,OnAreaDeselected);

        }
        
        void OnDisable()
        {
            GetService<IAreaService>().UnregisterArea(_index,gameObject);
            Unsubscribe(GameEvents.ON_AREAS_SELECTED,OnAreaSelected);
            Unsubscribe(GameEvents.ON_AREAS_DESELECTED,OnAreaDeselected);

        }
        
        private void OnMouseDown()
        {
            _isSelected = true;
            Fire(GameEvents.ON_AREA_CLICK,new AreaClickEvent(){index = _index});
        }

        private void OnAreaSelected(IEvent obj)
        {
            _isSelected = false;
        }
        private void OnAreaDeselected(IEvent obj)
        {
            _isSelected = false;
        }
    }
}
