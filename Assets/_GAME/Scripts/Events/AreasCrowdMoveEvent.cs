using CbkSDK.Core.Event.Interface;

namespace _GAME.Scripts.Events
{
    public class AreasCrowdMoveEvent : IEvent
    {
        public int first = -1;
        public int second = -1;
        public int moveCount = 0;

    }
}