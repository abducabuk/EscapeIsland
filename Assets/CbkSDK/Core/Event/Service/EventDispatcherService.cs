using System.Collections.Generic;
using CbkSDK.Core.Event.Interface;
using CbkSDK.Core.ServiceLocator.Attribute;
using UnityEngine.Events;

namespace CbkSDK.Core.Event.Service
{
    [Service]
    public class EventDispatcherService: IEventDispatcherService
    {
        [System.Serializable] private class GameEvent:UnityEvent<IEvent>{}

        private Dictionary<string, GameEvent> _eventGroups=new Dictionary<string, GameEvent>();
        public void Initialize()
        {
            
        }
        
        public void Subscribe(string eventName, UnityAction<IEvent> listener)
        {
            if(!_eventGroups.ContainsKey(eventName))
            {
                _eventGroups.Add(eventName,new GameEvent());
            }
            _eventGroups[eventName].AddListener(listener);

        }

        public void Unsubscribe(string eventName, UnityAction<IEvent> listener)
        {
            if(!_eventGroups.ContainsKey(eventName))
            {
                _eventGroups.Add(eventName,new GameEvent());
            }
            _eventGroups[eventName].RemoveListener(listener);

        }

        public void Fire(string eventName, IEvent e = null)
        {
            if(!_eventGroups.ContainsKey(eventName))
            {
                _eventGroups.Add(eventName,new GameEvent());
            }
            _eventGroups[eventName].Invoke(e);

        }
        
    }
}