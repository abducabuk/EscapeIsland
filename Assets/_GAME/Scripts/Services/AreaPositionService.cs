using CbkSDK.Core.ServiceLocator;
using CbkSDK.Core.ServiceLocator.Attribute;
using CbkSDK.Core.ServiceLocator.Interface;
using UnityEngine;

namespace _GAME.Scripts.Services
{

    public interface IAreaPositionService : IService
    {
        public Vector3[] GetPath(Vector3 from, Vector3 to);
        public float PathDistance(Vector3[] path);
        public Vector3 PositionOfArea(int index);
        public Vector3 PositionOfStickman(int areaIndex,int squadIndex,int stickmanIndex);
        
    }


    [Service(false)]
    public class AreaPositionService : BaseService ,IAreaPositionService
    {
        private const float STICKMAN_INTERVAL_HORIZONTAL = 1.4F;
        private const float STICKMAN_INTERVAL_VERTICAL = 1.1F;

        private const float STICKMAN_FIRST_SQUAD_OFFSET_HORIZONTAL = 2.2F;
        private const float STICKMAN_FIRST_SQUAD_OFFSET_VERTICAL = 1.6F;

        private const float PATH_OFFSET_MULTIPLIER = 2.5f;
        private const float PATH_OFFSET_ROOT = 3.3f;
        
        private IAreaService _areaService;
        public void Initialize()
        {
            _areaService = GetService<IAreaService>();
        }

        public Vector3[] GetPath(Vector3 firstPos, Vector3 secondPos)
        {
            var distanceSqrt = Mathf.Pow((secondPos - firstPos).magnitude,1f/PATH_OFFSET_ROOT);
            var firstPos2 = firstPos + Vector3.right* Mathf.Sign(firstPos.x)*-PATH_OFFSET_MULTIPLIER * distanceSqrt;
            var secondPos2 = secondPos + Vector3.right * Mathf.Sign(secondPos.x) * -PATH_OFFSET_MULTIPLIER * distanceSqrt;
            return new Vector3[] { firstPos, firstPos2, secondPos2, secondPos };
        }

        public float PathDistance(Vector3[] path)
        {
            var distance = 0f;
            for (var i = 1; i < path.Length; i++)
            {
                distance += (path[i]-path[i - 1]).magnitude;
            }

            return distance;
        }

        public Vector3 PositionOfArea(int index)
        {
            return _areaService.Get(index).transform.position;
        }

        public Vector3 PositionOfStickman(int areaIndex, int squadIndex, int stickmanIndex)
        {
            var areaObj = _areaService.Get(areaIndex).transform;
            var localPos =    Vector3.left * STICKMAN_FIRST_SQUAD_OFFSET_HORIZONTAL +
                                    Vector3.right * squadIndex * STICKMAN_INTERVAL_HORIZONTAL +
                                    Vector3.back * STICKMAN_FIRST_SQUAD_OFFSET_VERTICAL+
                                    Vector3.forward * stickmanIndex * STICKMAN_INTERVAL_VERTICAL;
            return areaObj.TransformPoint(Mathf.Sign(areaObj.transform.position.x)*-localPos);

        }

    }
}