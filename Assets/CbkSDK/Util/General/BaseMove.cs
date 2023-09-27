using System;
using System.Collections.Generic;
using UnityEngine;

namespace CbkSDK.Util.General
{
    public abstract class BaseMove : MonoBehaviour
    {
        public enum MoveStorePosition
        {
            Awake,
            OnEnable,
            OnDisable,
            FirstUpdateAfterAwake,
            FirstUpdateAfterOnEnable
        }
    
        [SerializeField] protected bool _instantMove = false;
        [SerializeField] private float _speed = 10f;
        [SerializeField] private Vector3 _destination;
        [SerializeField] protected Vector3 _startPos;
        [SerializeField] private List<MoveStorePosition> _storePositionMethods = new List<MoveStorePosition>() { MoveStorePosition.Awake };
    
        public abstract Vector3 CurrentPosition { get; set; }

        public Vector3 Destination => _destination;

        public float DistanceToDestination => Vector3.Distance(_destination, CurrentPosition);

        public bool IsMoving => enabled && !Mathf.Approximately(DistanceToDestination ,0);

        public bool InstantMove
        {
            get => _instantMove;
            set => _instantMove = value;
        }

        private void Awake()
        {
            if(_storePositionMethods.Contains(MoveStorePosition.Awake)) StorePosition();
            if(_storePositionMethods.Contains(MoveStorePosition.FirstUpdateAfterAwake)) this.StartWaitForEndOfFrameCoroutine(StorePosition);
        }
    

        private void OnEnable()
        {
            if(_storePositionMethods.Contains(MoveStorePosition.OnEnable)) StorePosition();
            if(_storePositionMethods.Contains(MoveStorePosition.FirstUpdateAfterOnEnable)) this.StartWaitForEndOfFrameCoroutine(StorePosition);

        }

        private void OnDisable()
        {
            if(_storePositionMethods.Contains(MoveStorePosition.OnDisable)) StorePosition();
        }

        private void Update()
        {
            var diff = _destination - CurrentPosition;
            var diffMagnitude = diff.magnitude;
            if (!Mathf.Approximately(diffMagnitude, 0))
            {
                var diffAbs = Mathf.Abs(diffMagnitude);
                var translate = _speed * Time.deltaTime;
                translate = !_instantMove && diffAbs > translate ? translate : diffAbs;
                CurrentPosition+= diff.normalized * translate;
            }

        }
    
        public void StorePosition()
        {
            var currentPos = CurrentPosition;
            _startPos = currentPos;
            _destination = currentPos;
        }


        public void SetDestination(Vector3 position)
        {
            _destination = position;
            if(_instantMove) Update();

        }

        public void ResetDestination()
        {
            _destination = _startPos;
            if(_instantMove) Update();
        }
        
        public void SetDestinationX(Single x)
        {
            var pos = _destination;
            pos.x = x;
            SetDestination(pos);
        }
        
        public void SetDestinationY(Single y)
        {
            var pos = _destination;
            pos.y = y;
            SetDestination(pos);
        }
        
        public void SetDestinationZ(Single z)
        {
            var pos = _destination;
            pos.z = z;
            SetDestination(pos);
        }
    

        public void SetDestinationX(int x) => SetDestinationX((Single) x);
        public void SetDestinationY(int y) => SetDestinationY((Single) y);
        public void SetDestinationZ(int z) => SetDestinationZ((Single) z);



    }
}
