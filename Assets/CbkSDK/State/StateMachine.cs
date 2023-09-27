using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace CbkSDK.State
{
    /// <summary>
    /// This state machine controls state changes. 
    /// </summary>
    public class StateMachine
    {
        private State _previous;
        private State _current;
        private List<State> _states;
        public readonly UnityEvent OnStateChanged=new UnityEvent();

        public State Current => _current;

        public State Previous => _previous;
        
        
        public StateMachine(params State[] states)
        {
            _states = new List<State>();
            if(states!=null && states.Any())
                foreach (var state in states)
                {
                    _states.Add(state);
                }
        }
        
        public void SetState(State newState)
        {
            if (_current != null && _current.Equals(newState)) return;

            _current?.Exit();
            _previous = _current;
            _current = newState;
            _current.Enter();
            OnStateChanged?.Invoke();
        }
        

        public void ExitState()
        {
            SetState(_previous);
        }
        

    }
}
