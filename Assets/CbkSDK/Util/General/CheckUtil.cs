using System;

namespace CbkSDK.Util.General
{
    public class CheckUtil
    {
        public bool result = false;

        private Action _onValueUpdate = null;
        private Action _onNotUpdate = null;


        public static CheckUtil Build<T>(ref T lastValue, T newValue) where T:IEquatable<T>
        {
            var isUpdate = !lastValue.Equals(newValue);
            if (isUpdate) lastValue = newValue;
            var check = new CheckUtil { result = isUpdate };
            return check;
        }
        public static CheckUtil Build<T>(T lastValue, T newValue) where T:IEquatable<T>
        {
            var isUpdate = !lastValue.Equals(newValue);
            var check = new CheckUtil { result = isUpdate };
            return check;
        }

        public CheckUtil Or<T>(ref T lastValue, T newValue) where T:IEquatable<T>
        {
            var isUpdate = !lastValue.Equals(newValue);
            if (isUpdate) lastValue = newValue;
            result |= isUpdate;
            return this;
        }
        
        public CheckUtil Or<T>(T lastValue, T newValue) where T:IEquatable<T>
        {
            var isUpdate = !lastValue.Equals(newValue);
            result |= isUpdate;
            return this;
        }
        public CheckUtil And<T>(ref T lastValue, T newValue) where T:IEquatable<T>
        {
            var isUpdate = !lastValue.Equals(newValue);
            if (isUpdate) lastValue = newValue;
            result &= isUpdate;
            return this;
        }
        
        public CheckUtil And<T>(T lastValue, T newValue) where T:IEquatable<T>
        {
            var isUpdate = !lastValue.Equals(newValue);
            result &= isUpdate;
            return this;
        }

        public CheckUtil OnValueUpdate(Action onValueUpdate)
        {
            _onValueUpdate = onValueUpdate;
            return this;
        }

        public CheckUtil OnNotUpdate(Action onNotUpdate)
        {
            _onNotUpdate = onNotUpdate;
            return this;
        }

        public bool Check()
        {
            if (result)
            {
                _onValueUpdate?.Invoke();
                return true;
            }
            else
            {
                _onNotUpdate?.Invoke();
                return false;
            }
        }
        
    }
}