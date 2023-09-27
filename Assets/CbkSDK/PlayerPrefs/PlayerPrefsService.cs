using CbkSDK.Core.ServiceLocator;
using CbkSDK.Core.ServiceLocator.Attribute;
using CbkSDK.Core.ServiceLocator.Interface;

namespace CbkSDK.PlayerPrefs
{
    public interface IPlayerPrefsService:IService
    {
        public string Save<T>(string key, T value);
        public string Save<T>(string key, params T[] values);
        public bool Load<T>(string key,out T value);
        public bool Load<T>(string key, out T[] values);

    }
    [Service(false)]
    public class PlayerPrefsService : BaseService,IPlayerPrefsService
    {
        public void Initialize()
        {
        
        }
    
        public string Save<T>(string key, T value)
        {
            return SaveLoadUtil.CreateSave(key).Save(value).Write();
        }

        public string Save<T>(string key, params T[] values)
        {
            var save = SaveLoadUtil.CreateSave(key);
            foreach (var value in values)
            {
                save.Save(value);
            }
            return save.Write();
        }

        public bool Load<T>(string key, out T value)
        {
            var load = SaveLoadUtil.Load(key);
            if(load!=null)
                return load.Next(out value);
            value = default(T);
            return false;
        }

        public bool Load<T>(string key, out T[] values)
        {
            var load = SaveLoadUtil.Load(key);
            if(load!=null)
            {
                var count = load.Count();
                values = new T[count];
                for (var i = 0; i < count ; i++)
                {
                    T value;
                    load.Next(out value);
                    values[i] = value;
                }
                return true;
            }
            values = default(T[]);
            return false;
        }


    }
}