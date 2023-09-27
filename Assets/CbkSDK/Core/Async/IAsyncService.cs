using System;
using System.Collections;
using CbkSDK.Core.ServiceLocator.Interface;
using UnityEngine;

namespace CbkSDK.Core.Async
{
    public interface IAsyncService : IService
    {
        Coroutine WaitForSecond(float seconds, Action onComplete);
        void ExecuteCoroutine(IEnumerator action);
        void ExecuteInFixedUpdate(System.Action action);
        void ExecuteInUpdate(System.Action action);
        void ExecuteInLateUpdate(System.Action action);
	    
    }
}