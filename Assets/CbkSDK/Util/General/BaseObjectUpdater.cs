using System;
using UnityEngine;

namespace CbkSDK.Util.General
{
    public abstract class BaseObjectUpdater<T> : MonoBehaviour where T: IEquatable<T>
    {
        [Tooltip("0f means Time.deltaTime")]
        [SerializeField,Range(0f,10f)] protected float _updateInterval = 0f;
        protected abstract T Value { get;}
        protected abstract void OnValueUpdate();

        protected T lastValue;
        protected T value;
        protected bool firstUpdate;

        private float _lastUpdate;
        protected virtual void OnEnable()
        {
            firstUpdate = true;
            _lastUpdate = Time.time;
            value = Value;
            UpdateValue();
            firstUpdate = false;
        }



        protected virtual void Update()
        {
            if(Time.time-_lastUpdate<_updateInterval) return;
            _lastUpdate = Time.time;
            value = Value;
            if (!lastValue.Equals(value))
            {
                UpdateValue();
            }
        }
    
        private void UpdateValue()
        {
            OnValueUpdate();
            lastValue = value;
        }
    }
}
