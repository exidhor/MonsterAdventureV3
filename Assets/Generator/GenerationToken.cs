using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure
{
    public class GenerationToken
    {
        private int _id;

        private Type _type;
        private object _value;

        public GenerationToken(int id, Type type)
        {
            _id = id;
            _type = type;
        }

        public GenerationToken(int id, Type type, object value)
            : this(id, type)
        {
            _value = value;
        }

        public int GetId()
        {
            return _id;
        }

        public object GetValue()
        {
            return _value;
        }

        public void SetValue(object value)
        {
            _value = value;
        }

        public Type GetType()
        {
            return _type;
        }
    }
}