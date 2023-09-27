using CbkSDK.Core.Event.Interface;
using CbkSDK.Core.ServiceLocator;
using UnityEngine.Events;

namespace CbkSDK.Core.Event
{
    public abstract class EventLayer : ServiceLayer, IEventLayer
    {
        private IEventDispatcherService _eventDispatcher;
        
        private IEventDispatcherService GetEventService()
        {
            return _eventDispatcher ??= GetService<IEventDispatcherService>();
        }

        public void Subscribe(string eventName, UnityAction<IEvent> listener)
        {
            GetEventService().Subscribe(eventName,listener);
        }

        public void Unsubscribe(string eventName, UnityAction<IEvent> listener)
        {
            GetEventService().Unsubscribe(eventName,listener);

        }

        public void Fire(string eventName, IEvent e = null)
        {
            GetEventService().Fire(eventName,e);

        }
    }
}