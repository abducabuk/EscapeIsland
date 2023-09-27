using UnityEngine;

namespace CbkSDK.Util.General
{
    public class LocalMove : BaseMove
    {
        public override Vector3 CurrentPosition
        {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }
    }
}