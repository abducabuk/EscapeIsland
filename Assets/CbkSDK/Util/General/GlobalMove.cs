using UnityEngine;

namespace CbkSDK.Util.General
{
    public class GlobalMove : BaseMove
    {
        public override Vector3 CurrentPosition
        {
            get => transform.position;
            set => transform.position = value;
        }
    }
}