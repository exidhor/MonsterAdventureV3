using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.Generation
{
    public class GenerationToken
    {
        private float _value;

        public GenerationToken()
        {
            // nothing ?
        }

        public GenerationToken(bool value)
        {
            SetValue(value);
        }

        public GenerationToken(float value)
        {
            _value = value;
        }

        public string GetDebugLabel(Type type)
        {
            if (type == typeof(float))
            {
                float roundedValue = (float) Math.Round((double) _value, 2);

                return roundedValue.ToString();
            }

            if (type == typeof(bool))
            {
                bool boolValue = GetBoolValue();

                return boolValue.ToString();
            }

            if (type == typeof(int))
            {
                int intValue = GetIntValue();

                return intValue.ToString();
            }

            // default
            return _value.ToString();
        }

        public void SetValue(float value)
        {
            _value = value;
        }

        public void SetValue(bool value)
        {
            if (value)
                _value = 0f;
            else
                _value = -1f;
        }

        public float GetFloatValue()
        {
            return _value;
        }

        public int GetIntValue()
        {
            return (int)_value;
        }

        public bool GetBoolValue()
        {
            if (GetIntValue() == -1)
                return false;

            return true;
        }
    }
}