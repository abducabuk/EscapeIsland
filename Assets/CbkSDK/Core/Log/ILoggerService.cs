using System.Collections.Generic;
using CbkSDK.Core.ServiceLocator.Interface;

namespace CbkSDK.Core.Log
{
    public interface ILoggerService: IService
    {
        Stack<LogHistory> LogHistories();
        void LogTrace(string msg);
        void Log(string msg);
        void LogWarning(string msg);
        void LogError(string msg);
    }
}