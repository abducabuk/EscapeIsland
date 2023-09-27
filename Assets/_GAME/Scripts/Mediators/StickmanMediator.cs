using System.Collections.Generic;
using _GAME.Scripts.Other;
using _GAME.Scripts.Services;
using CbkSDK.Core.View;
using CbkSDK.Util.General;
using UnityEngine;

namespace _GAME.Scripts.Mediators
{
    public class StickmanMediator : BaseMediator
    {
        [SerializeField] private ColorTypeObject _colorTypeObject;
        private IAreaPositionService _areaPositionService;
        private IAreaCrowdService _areaCrowdService;
        private IAreaService _areaService;
        private int _oldAreaIndex = -1;
        private int _areaIndex=-1;
        private int _squadIndex=-1;
        private int _stickmanIndex=-1;
        private float _movementTime = 0;
        private List<Vector3> _path = new List<Vector3>();

        private void OnEnable()
        {
            _areaPositionService = GetService<IAreaPositionService>();
            _areaCrowdService = GetService<IAreaCrowdService>();
            _areaService = GetService<IAreaService>();
        }
        
        private void Update()
        {
            UpdatePosition();
            UpdateAreaSelected();
        }

        private void UpdateAreaSelected()
        {
            //TODO Need more efficient
            var isSelected = _areaService.Get(_areaIndex).GetComponent<AreaMediator>().IsSelected;
            _colorTypeObject.GetComponent<LocalMove>().SetDestinationY(_path.Count<=1 && isSelected?AreaUpDown.UP_HEIGHT:0);
        }
        
        
        public void SetType(ColorType stickmanType)
        {
            _colorTypeObject.SetCurrent(stickmanType);
        }

        public void SetDestination(int areaIndex, int squadIndex, int stickmanIndex,float delay)
        {
            _oldAreaIndex = _areaIndex;
            _areaIndex = areaIndex;
            _squadIndex = squadIndex;
            _stickmanIndex = stickmanIndex;
            var d = _path.Count == 0 ? delay : 0.25f;
            _movementTime = Time.time + d ;
            UpdatePath();
        }
        public void SetDestinationImmediate(int areaIndex, int squadIndex, int stickmanIndex)
        {
            _oldAreaIndex = areaIndex;
            _areaIndex = areaIndex;
            _squadIndex = squadIndex;
            _stickmanIndex = stickmanIndex;
            UpdatePositionImmediate();
        }
        private void UpdatePath()
        {
            var destPos= _areaPositionService.PositionOfStickman(_areaIndex, _squadIndex, _stickmanIndex);
            var currentPos = transform.position;
            var oldAreaPosition = _areaPositionService.PositionOfArea(_oldAreaIndex);
            var newAreaPosition = _areaPositionService.PositionOfArea(_areaIndex);
            var path = _areaPositionService.GetPath(oldAreaPosition,newAreaPosition);
            path[0] = _path.Count>0? _path[0]:currentPos;
            path[path.Length - 1] = destPos;
            _path.Clear();
            _path.AddRange(path);

        }

        
        private void UpdatePosition()
        {
            if(Time.time<_movementTime) return;
            if(_path.Count==0) return;
            var currentPos = transform.position;
            var diff = _path[0] - currentPos;
            if (diff.magnitude > 0.1f)
            {
                var translate = diff.normalized * Time.deltaTime * _areaCrowdService.StickmanSpeed;
                if (translate.magnitude > diff.magnitude)
                    translate = diff;
                transform.position += translate ;
            }
            else
            {
                _path.RemoveAt(0);
            }
        }
        
        private void UpdatePositionImmediate()
        {
            transform.position = _areaPositionService.PositionOfStickman(_areaIndex, _squadIndex, _stickmanIndex);
        }
        

    }
}