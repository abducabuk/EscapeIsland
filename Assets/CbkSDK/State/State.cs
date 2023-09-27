using UnityEngine.Events;

namespace CbkSDK.State
{
    public abstract class State
    {
        public UnityEvent OnEnter=new UnityEvent();
        public UnityEvent OnExit=new UnityEvent();

        protected abstract void OnEntered();
        protected abstract void OnExited();

        public void Enter()
        {
            OnEntered();
            OnEnter.Invoke();
        }

        public void Exit()
        {
            OnExited();
            OnExit.Invoke();
        }
    }
}
