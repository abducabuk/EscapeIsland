namespace CbkSDK.Core.ServiceLocator.Interface
{
    public interface IServiceLayer
    {
        T GetService<T>() where T : IService;
    }
}