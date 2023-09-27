using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace CbkSDK.PlayerPrefs
{
    public class SaveLoadUtil: SaveLoadUtil.ISave,SaveLoadUtil.ILoad
    {
        private string _key = string.Empty;
        private SaveLoadModel _saved = new SaveLoadModel();
        private int _iterate = 0;
        private string _json;
    
    
        public static ISave CreateSave(string key)
        {
            return new SaveLoadUtil(){_key = key};
        }
    
        public static ILoad Load(string key)
        {
            var json = UnityEngine.PlayerPrefs.GetString(key,string.Empty);
            if (json.Equals(string.Empty))
            {
                return null;
            }
            var saveLoadModel = JsonUtility.FromJson<SaveLoadModel>(json);
            return new SaveLoadUtil()
            {
                _key = key,
                _saved = saveLoadModel,
                _json = json
            };
        }

        public int ResetIndex() => _iterate = 0;
    
        public int Count()
        {
            return _saved.values.Count;
        }

        public string Json()
        {
            return _json;
        }


        public bool TryGet(out string value, int index)
        {
            value = string.Empty;
            if (index >= _saved.values.Count) return false;
            value = _saved.values[index];
            return true;
        }

        public bool TryGet<T>(out T value,int index)
        {
            value = default(T);
            if (index >= _saved.values.Count) return false;
            try
            {
                value = Get<T>(index);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return false;
            }
        }

        public T Get<T>(int index)
        {
            if (typeof(T) == typeof(Vector3))
            {
                var vector = StringToVector3(_saved.values[index]);
                return (T) Convert.ChangeType(vector,typeof(T)) ;
            }
            return (T) Convert.ChangeType(_saved.values[index],typeof(T)) ;
        }
    
        public string Get(int index)
        {
            return _saved.values[index];
        }

        public bool Next<T>(out T value)
        {
            var state = TryGet<T>(out value, _iterate);
            if (state) _iterate++;
            return state;
        }

        public bool Next(out string value)
        {
            var state = TryGet(out value, _iterate);
            if (state) _iterate++;
            return state;
        }

        public ISave Save<T>(T save)
        {
            _saved.values.Add(save.ToString());
            return this;
        }
    
        public ISave Save<T>(T save,int index)
        {
            _saved.values[index] = save.ToString();
            return this;
        }
    
        public ISave Clear()
        {
            _saved.values.Clear();
            return this;
        }

        public string Write()
        {
            _json = JsonUtility.ToJson(_saved);
            UnityEngine.PlayerPrefs.SetString(_key,_json);
            return _json;
        }
    

        public static Vector3 StringToVector3(string sVector)
        {
            // Remove the parentheses
            if (sVector.StartsWith ("(") && sVector.EndsWith (")")) {
                sVector = sVector.Substring(1, sVector.Length-2);
            }
 
            // split the items
            string[] sArray = sVector.Split(',');
 
            // store as a Vector3
            Vector3 result = new Vector3(
                float.Parse(sArray[0],CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(sArray[1],CultureInfo.InvariantCulture.NumberFormat),
                float.Parse(sArray[2],CultureInfo.InvariantCulture.NumberFormat));
 
            return result;
        }
    
    
        [System.Serializable]
        private class SaveLoadModel
        {
            public List<string> values = new List<string>();
        }

        public interface ISave
        {
            int ResetIndex();
            ISave Save<T>(T save);
            ISave Save<T>(T save, int index);
            ISave Clear();

            string Write();

        }
    
        public interface ILoad
        {
            int ResetIndex();
            int Count();
            string Json();
            T Get<T>(int index);
            string Get(int index);
        
            bool TryGet(out string value, int index);
            bool TryGet<T>(out T value, int index);
            bool Next<T>(out T value);
            bool Next(out string value);

        }
    }
}
