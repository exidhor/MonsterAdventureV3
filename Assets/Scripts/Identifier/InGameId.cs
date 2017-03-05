using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    [Serializable]
    public class InGameId
    {
        private static int bufferIncrement = 0;

        public static readonly InGameIdValue ALIVE = new InGameIdValue(MathHelper.PowerOfTwo(bufferIncrement++));
        public static readonly InGameIdValue DEAD = InGameIdValue.ConstructInverse(ALIVE);

        public static readonly InGameIdValue MINERAL = new InGameIdValue(MathHelper.PowerOfTwo(bufferIncrement++));
        public static readonly InGameIdValue ORGANIC = InGameIdValue.ConstructInverse(MINERAL);

        // ...

        [SerializeField] private Int32 _id;
        [SerializeField] private Int32 _idMask;

        public bool Contains(InGameIdValue value)
        {
            return CheckBit(value.Value) == !value.IsInverse;
        }

        public bool Contains(InGameId value)
        {
            return CheckBit(value._idMask);
        }

        public void Set(InGameIdValue value, bool state)
        {
            Set(value.Value, state);
        }

        private void Set(int value, bool state)
        {
            if (state)
            {
                AddBit(value);
            }
            else
            {
                ResetBit(value);
            }
        }

        private bool CheckBit(int value)
        {
            return (_idMask & value) == value;
        }

        private void AddBit(int value)
        {
            _idMask |= value;
        }

        private void ResetBit(int value)
        {
            int inverse = ~value;
            _idMask &= inverse;
        }

        public int GetMask()
        {
            return _idMask;
        }

        /// <summary>
        /// Return all objects which have the given value from the given list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">CAREFUL : these object has to have a "InGameIdComponent"</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<T> Filter<T>(List<T> list, InGameId value)
            where T : MonoBehaviour
        {
            // copy the list to not affect the old
            List<T> filtered = new List<T>(list);

            for (int i = 0; i < filtered.Count; i++)
            {
                // if the object doesnt have the given value
                if (!filtered[i].GetComponent<InGameIdComponent>().Id.Contains(value))
                {
                    // remove it
                    filtered.RemoveAt(i);
                    i--;
                }
            }

            return filtered;
        }
    }
}