using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CbkSDK.Util.Tutorial.Script
{
    public class TutorialClick : BaseTutorial
    {
        [SerializeField] protected Image _mask;
        [SerializeField] protected Image _hand;
        [SerializeField] protected GameObject _disablerCollider;
        [SerializeField] protected bool isMask = true;
        [SerializeField] protected bool isScaleAnimation;
        [SerializeField] protected bool isMoveAnimation;
        [SerializeField] protected float moveDuration=2f;
        [SerializeField] protected float moveWaitDuration=0.25f;
        [SerializeField] protected bool disablerColliderRemoveAfterMove = false;
        [SerializeField] protected Transform startObject;
        [SerializeField] protected Transform endObject;
        [SerializeField] protected bool finishTutorial;
        [SerializeField] protected bool isActivated = false;
        private Vector3 StartPos => !startObject.TryGetComponent(out RectTransform rt) && Camera.main
            ? Camera.main.WorldToScreenPoint(startObject.transform.position)
            : startObject.transform.position;

        private Vector3 EndPos => !endObject.TryGetComponent(out RectTransform rt) && Camera.main
            ? Camera.main.WorldToScreenPoint(endObject.transform.position)
            : endObject.transform.position;
    
    
        protected override bool TutorialNotStartCondition()
        {
            return false;
        }

        protected override bool TutorialStartCondition()
        {
            return true;//GameManager.Instance.IsPlaying;
        }

        protected override bool TutorialEndCondition()
        {
            return finishTutorial;
        }

        protected override void OnTutorialStart()
        {
            if(!_hand.gameObject.activeInHierarchy) _hand.gameObject.SetActive(true);
            _mask.gameObject.SetActive(isMask);
            if(_disablerCollider)_disablerCollider.SetActive(true);

            if (isScaleAnimation)
            {
                ScaleAnimation();
            }
        
            if (isMoveAnimation) 
                StartCoroutine(MoveAnimation(ActivateTutorial));
            else
            {
                var startPos = StartPos;
                _hand.transform.position = startPos;
                _mask.transform.position = startPos;
                ActivateTutorial();
            }
        }

        private void ActivateTutorial()
        {
            isActivated = true;
            if(isMoveAnimation && _disablerCollider) _disablerCollider.SetActive(!disablerColliderRemoveAfterMove);
        }
    
        protected override void OnTutorialEnd()
        {
        
        }


        protected virtual void Awake()
        {
            _mask.alphaHitTestMinimumThreshold = _mask.color.a;
            _mask.gameObject.SetActive(false);
            _hand.gameObject.SetActive(false);
            if (_disablerCollider)
            {
                if(!_disablerCollider.activeSelf) 
                    Destroy(_disablerCollider.gameObject);
                else 
                    _disablerCollider.SetActive(false);
            }
            var startPos = StartPos;
            _mask.transform.position = startPos;
            _hand.transform.position = startPos;
        }

        protected virtual void Update()
        {
            if(!isActivated) return;
        
            if (Input.GetMouseButtonDown(0))
            {
                finishTutorial = true;
            }
        }
    
        private IEnumerator MoveAnimation(Action onMoveEndPosition=null)
        {
            var startPos = StartPos;
            _hand.transform.position = startPos;
            _mask.transform.position = startPos;
            yield return new WaitForSecondsRealtime(moveWaitDuration);
            var moveStartTime = Time.unscaledTime;
            while (!TutorialEndCondition())
            {
                var currentDifF = EndPos - _hand.transform.position;
                if (currentDifF.magnitude < 0.1f)
                {
                    onMoveEndPosition?.Invoke();
                    yield return new WaitForSecondsRealtime(moveWaitDuration);
                    startPos = StartPos;
                    _hand.transform.position = startPos;
                    _mask.transform.position = startPos;
                    yield return new WaitForSecondsRealtime(moveWaitDuration);
                    moveStartTime = Time.unscaledTime;
                }
                var lerp = (Time.unscaledTime-moveStartTime)/moveDuration;
                var pos = Vector3.Lerp(StartPos, EndPos, lerp);
                _hand.transform.position = pos;
                _mask.transform.position = pos;
                yield return null;
            }
        
        }
    
        private void ScaleAnimation()
        {
            var seq = DOTween.Sequence();
            seq.Append(_hand.transform.DOScale(1.5f, 0.5f));
            seq.Append(_hand.transform.DOScale(1f, 0.5f));
            seq.SetLoops(-1);
            seq.SetUpdate(true);
            seq.SetLink(_hand.gameObject);
        }
    }
}
