using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CbkSDK.Util.General
{
    public class ButtonClickScaleDown : MonoBehaviour
    {
        public static float SCALE_DOWN = 0.95f;
        public static float DURATION = .1f;

        [SerializeField] private Button _button;
        [SerializeField] private bool _showDownState = true;
        private Vector3 _defaultScale;

        private void Reset()
        {
            _button = GetComponentInChildren<Button>();
        }

        private void Awake()
        {
            if (!_button) Reset();
            _button.onClick.AddListener(OnClick);
            _defaultScale = transform.localScale;
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClick);
        }
    
        private void OnDisable()
        {
            Complete();
            transform.localScale = _defaultScale;
        }

        private bool isDown;
        private void Update()
        {
            if (_showDownState)
            {
                var current = EventSystem.current;
                if (Input.GetMouseButtonDown(0) && _button.gameObject.Equals(current.currentSelectedGameObject))
                {
                    isDown = true;
                    Complete();
                    transform.localScale = _defaultScale;
                    _downTween = transform.DOScale(_defaultScale * SCALE_DOWN, DURATION * 0.5f);
                }
                else if (Input.GetMouseButtonUp(0) && isDown)
                {
                    isDown = false;
                    Complete();
                    transform.localScale = _defaultScale * SCALE_DOWN;
                    _upTween= transform.DOScale(_defaultScale, DURATION * 0.5f);
                }
            }
        }

        private Tween _upTween;
        private Tween _downTween;
        private Sequence _sequence;

        private void OnClick()
        {
            if (!_showDownState)
            {
                Complete();
                var scale = transform.localScale;
                _sequence = DOTween.Sequence();
                _sequence.Append(transform.DOScale(scale * SCALE_DOWN, DURATION * 0.5f));
                _sequence.Append(transform.DOScale(scale,DURATION*0.5f));
            }
        }
    
        private void Complete()
        {
            if (_upTween.IsActive()) _upTween.Complete();
            if (_downTween.IsActive()) _downTween.Complete();
            if (_sequence.IsActive()) _sequence.Complete();
        }
    }
}
