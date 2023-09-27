using System.Collections.Generic;
using System.Linq;
using CbkSDK.Core.Event.Interface;
using CbkSDK.Core.View;
using CbkSDK.Level;
using UnityEngine;

namespace _GAME.Scripts.Other
{
    public class FloorMediator : BaseMediator
    {
        [SerializeField] private List<GameObject> _objects=new List<GameObject>();
        private ILevelService _levelService;
        void OnEnable()
        {
            _levelService = GetService<ILevelService>();
            Subscribe(LevelEvents.ON_LEVEL_INIT,OnRandom);
        }
        void OnDisable()
        {
            Unsubscribe(LevelEvents.ON_LEVEL_INIT,OnRandom);
        }
    
        private void OnRandom(IEvent obj)
        {
            var index = (_levelService.Level - 1) % _objects.Count();
            for (var i = 0; i < _objects.Count; i++)
            {
                var o = _objects[i];
                o.SetActive(i==index);
            }
        }

    }
}
