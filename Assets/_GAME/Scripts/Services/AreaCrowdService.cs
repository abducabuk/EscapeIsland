using System.Collections.Generic;
using System.Linq;
using CbkSDK.Core.ServiceLocator;
using CbkSDK.Core.ServiceLocator.Attribute;
using CbkSDK.Core.ServiceLocator.Interface;
using UnityEngine;
using UnityEngine.Rendering;

namespace _GAME.Scripts.Services
{

    public enum ColorType
    {
        BLACK,
        BLUE,
        CYAN,
        ORANGE,
        PINK,
        PURPLE,
        RED,
        VIOLET,
        WHITE
        

    }
    public interface IAreaCrowdService : IService
    {
        public SerializedDictionary<int, List<ColorType>> AreaCrowds { get;}
        public int CalculateMoveCount(int from,int to);

        public void Move(int from,int to,int moveCount);
        public int Move(int from,int to);

        public ColorType AreaType(int index);

        public bool IsAreaSolved(int index);

        public bool IsAreaEmpty(int index);

        public bool IsAreaFilled(int index);
        

        public bool IsAllAreaSolved();
        
        public int SquadPerArea { get; }
        public int StickmanPerSquad { get; }
        
        public float StickmanSpeed { get; }

        
    }


    [Service(false)]
    public class AreaCrowdService : BaseMonoService ,IAreaCrowdService
    {
        private const float STICKMAN_SPEED = 22.5f;
        private const int SQUAD_COUNT = 4;
        private const int STICKMAN_PER_SQUAD = 4;

        public SerializedDictionary<int, List<ColorType>>
            _areaCrowds = new SerializedDictionary<int, List<ColorType>>();
        
        public void Initialize()
        {

        }

        public SerializedDictionary<int, List<ColorType>> AreaCrowds => _areaCrowds;
        public int CalculateMoveCount(int from, int to)
        {
            var fromList = _areaCrowds[from];
            var toList = _areaCrowds[to];

            if (fromList.Count == 0) return 0;
            if (toList.Count == SQUAD_COUNT) return 0;
            var moveType = toList.Count > 0 ? toList.Last():fromList.Last();
            var moveCount = 0;
            var tempList = fromList.ToList();
            while (tempList.Any() && tempList.Last()==moveType)
            {
                moveCount++;
                tempList.RemoveAt(tempList.Count-1);
            }
            moveCount = Mathf.Clamp(moveCount, 0, SQUAD_COUNT - toList.Count);
            return moveCount;
        }
        public void Move(int from, int to, int moveCount)
        {
            var fromList = _areaCrowds[from];
            var toList = _areaCrowds[to];
            
            for (var i = 0; i < moveCount; i++)
            {
                toList.Add(fromList.Last());
                fromList.RemoveAt(fromList.Count-1);
            }
        }
        public int Move(int from, int to)
        {
            var moveCount = CalculateMoveCount(from,to);
            Move(from, to, moveCount);
            return moveCount;
        }
        
        public ColorType AreaType(int index)
        {
            return _areaCrowds[index].First();
        }
        
        public bool IsAreaSolved(int index)
        {
            return _areaCrowds[index].Count == SQUAD_COUNT &&
                   _areaCrowds[index].All(i => i == _areaCrowds[index].First());
        }

        public bool IsAreaEmpty(int index)
        {
            return _areaCrowds[index].Count == 0;
        }
        public bool IsAreaFilled(int index)
        {
            return _areaCrowds[index].Count == SQUAD_COUNT;
        }
        public bool IsAllAreaSolved()
        {
            var areaCrowdsKeys = _areaCrowds.Keys;
            foreach (var index in areaCrowdsKeys)
            {
                if (IsAreaEmpty(index) || IsAreaSolved(index))
                {
                    continue;
                }
                return false;
            }

            return true;
        }
        public int SquadPerArea => SQUAD_COUNT;
        public int StickmanPerSquad => STICKMAN_PER_SQUAD;
        public float StickmanSpeed => STICKMAN_SPEED;

    }
}