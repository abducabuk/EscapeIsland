using UnityEngine.Events;

namespace CbkSDK.Core.Event.Interface
{
    public interface IEventLayer
    {
        void Subscribe(string eventName, UnityAction<IEvent> listener);
        void Unsubscribe(string eventName, UnityAction<IEvent> listener);
        void Fire(string eventName, IEvent e = null);
    }
}