namespace CbkSDK.Core.Event.Attribute
{
    public class BindEventAttribute : System.Attribute
    {
        private readonly string _eventName;

        public BindEventAttribute(string eventName)
        {
            _eventName = eventName;
        }

        public string EventName
        {
            get { return _eventName; }
        }
    }
}