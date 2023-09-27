using CbkSDK.Core.Event.Interface;
using CbkSDK.Core.View;

namespace CbkSDK.UI.Scripts
{
    public abstract class BaseStateUIPanelMediator : BaseMediator
    {
        protected abstract string EnterEventName { get; }
        protected abstract string ExitEventName { get; }
    
        protected abstract bool InState { get; }

        private void Start()
        {
            Subscribe(EnterEventName,OnEnter);
            Subscribe(ExitEventName,OnExit);
            gameObject.SetActive(InState);
        }
        private void OnDestroy()
        {
            Unsubscribe(EnterEventName,OnEnter);
            Unsubscribe(ExitEventName,OnExit);
        }

        private void OnExit(IEvent e)
        {
            gameObject.SetActive(false);
        }

        private void OnEnter(IEvent e)
        {
            gameObject.SetActive(true);
        }
    }
}
