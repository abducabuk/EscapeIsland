using UnityEngine;

namespace _GAME.Scripts.Other
{
    public class StickmanAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private Vector3 _lastPosition;
        private bool _isRunning = false;
        
        private void Update()
        {
            var diff = transform.position - _lastPosition;
            diff.y = 0f;
            var running = diff.magnitude > float.Epsilon;
            if (_isRunning !=running)
            {
                _animator.SetTrigger(running?"isRunning":"isIdle");
                _isRunning = running;
            }

            var lookAt = running ? diff : Mathf.Sign(transform.position.x) * Vector3.left;
            var smothLookAt = Vector3.Slerp(transform.forward, lookAt, Time.deltaTime*10);
            transform.rotation = Quaternion.LookRotation(smothLookAt);
            _lastPosition = transform.position;
        }
    }
}