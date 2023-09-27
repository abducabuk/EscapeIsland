using System.Collections.Generic;
using System.Linq;
using _GAME.Scripts.Data;
using _GAME.Scripts.Events;
using CbkSDK.Core.ServiceLocator;
using CbkSDK.Core.ServiceLocator.Attribute;
using CbkSDK.Core.ServiceLocator.Interface;
using CbkSDK.Pooling;
using UnityEngine;

namespace _GAME.Scripts.Services
{
    public interface IAreaCrowdFillService : IService
    {
        public void Fill(int level);
    }
    [Service(false)]
    public class AreaCrowdFillService : BaseService,IAreaCrowdFillService
    {
        public const int MAX_AREA_COUNT = 12;
        [SerializeField] private AreaLevelData[] _levels;
        private IAreaCrowdService _areaCrowdService;
        private IAreaService _areaService;
        public void Initialize()
        {
            _levels = Resources.LoadAll<AreaLevelData>("").OrderBy(i=>i.level).ToArray();
            _areaCrowdService = GetService<IAreaCrowdService>();
            _areaService = GetService<IAreaService>();
        }

        public void Fill(int level)
        {
            var areaCrowds = _areaCrowdService.AreaCrowds;
            areaCrowds.Clear();
            for (var i = 0; i < MAX_AREA_COUNT; i++)
            {
                var areaInfo = _levels[level%_levels.Length].areas.Where(a=>a.index==i);
                var isAreaOpen = areaInfo.Any();
                areaCrowds.Add(i,isAreaOpen?areaInfo.First().crowds.ToList():new List<ColorType>());
                _areaService.Get(i).SetActive(isAreaOpen);
            }
        }
    }
}