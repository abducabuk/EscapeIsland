using System;
using UnityEngine;

namespace CbkSDK.Util.General
{
    [System.Serializable]
    public class CheckValue<T>: ISerializationCallbackReceiver  where T:IEquatable<T>
    {
        [SerializeField] private T _val;
        private Action<T> _onValueUpdate;
        
        public T Value
        {
            get => _val;
            set
            {
                if (!_val.Equals(value))
                {
                    _val = value;
                    Update();
                }
            }
        }

        public void OnValueUpdate(Action<T> onValueUpdate=null)
        {
            _onValueUpdate = onValueUpdate;
            Update();
        }
        
        private void Update()
        {
            _onValueUpdate?.Invoke(_val);
        }

        public void OnAfterDeserialize()
        {
            Update();
        }

        public void OnBeforeSerialize()
        {
        }
    }
#if UNITY_EDITOR
    [UnityEditor.CustomPropertyDrawer(typeof(CheckValue<>))]
    public class CheckValueDrawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
        {
            UnityEditor.EditorGUI.BeginProperty(position, label, property);
            var indent = UnityEditor.EditorGUI.indentLevel;
            UnityEditor.EditorGUI.indentLevel = 0;
            var amountRect = new Rect(position.x, position.y, position.width, position.height);
            var valProp = property.FindPropertyRelative("_val");
            if (Attribute.GetCustomAttribute(fieldInfo, typeof(RangeAttribute)) is RangeAttribute range)
            {
                if (valProp.propertyType == UnityEditor.SerializedPropertyType.Float)
                    UnityEditor.EditorGUI.Slider(position, valProp, range.min, range.max, label);
                else if (valProp.propertyType == UnityEditor.SerializedPropertyType.Integer)
         
                    UnityEditor.EditorGUI.IntSlider(position, valProp, (int)range.min, (int)range.max, label);
                else
                    UnityEditor.EditorGUI.PropertyField(amountRect,valProp, label);
            }
            else
            {
                UnityEditor.EditorGUI.PropertyField(amountRect,valProp, label);
            }
            
            UnityEditor.EditorGUI.indentLevel = indent;
            UnityEditor.EditorGUI.EndProperty();
        }
    }
#endif


}