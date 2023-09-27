using CbkSDK.Core.ServiceLocator;
using CbkSDK.Core.ServiceLocator.Attribute;
using CbkSDK.Core.ServiceLocator.Interface;
using UnityEngine;
using UnityEngine.Rendering;

namespace _GAME.Scripts.Services
{

    public interface IAreaService : IService
    {
        public void RegisterArea(int index,GameObject areaObj);
        public void UnregisterArea(int index,GameObject areaObj);
        public GameObject Get(int index);
        public int LastClicked { get; set; }
    }


    [Service(false)]
    public class AreaService : BaseMonoService ,IAreaService
    {
        private SerializedDictionary<int,GameObject> _areas=new SerializedDictionary<int, GameObject>();

        public void Initialize()
        {

        }

        public void RegisterArea(int index, GameObject areaObj)
        {
            if (_areas.ContainsKey(index))
            {
                LogError("There is multiple same index area in this index: "+index);
            }
            else
            {
                _areas.Add(index,areaObj);
            }
        }

        public void UnregisterArea(int index, GameObject areaObj)
        {
            if (!_areas.ContainsKey(index))
            {
                LogError("There is no area in this index: "+index);
            }
            else
            {
                _areas.Remove(index);
            }
        }

        public GameObject Get(int index)
        {
            if (!_areas.ContainsKey(index))
            {
                LogError("There is multiple same index area in this index: "+index);
                return null;
            }

            return _areas[index];
        }

        public int LastClicked { get; set; } = -1;
    }
}