﻿using CbkSDK.Core.Command.Interface;
using CbkSDK.Core.Event;
using CbkSDK.Core.Event.Interface;
using UnityEngine;
using UnityEngine.Events;

namespace CbkSDK.Core.Command
{
    [System.Serializable]
    public class CommandEvent: UnityEvent<ICommand>{}
    public abstract class BaseCommand : EventLayer, ICommand
    {
        protected abstract void OnExecute(IEvent e = null);
        public void Execute(IEvent e = null)
        {
            LogTrace(GetType().Name +" " +nameof(Execute)+ " (Time:"+ Time.time+"ms)");
            OnExecute(e);
        }

        public CommandEvent OnCompleted { get; set; } = new CommandEvent();

        public CommandEvent OnCanceled { get; set; } = new CommandEvent();
        
        public virtual void Complete()
        {
            LogTrace(GetType().Name +" " + nameof(Complete) + " at (Time:"+ Time.time+"ms)");
            OnCompleted.Invoke(this);
        }
        
        public virtual void Cancel()
        {
            LogTrace(GetType().Name +" " + nameof(Cancel) + " (Time:"+ Time.time+"ms)");
            OnCanceled.Invoke(this);
        }
    }
}