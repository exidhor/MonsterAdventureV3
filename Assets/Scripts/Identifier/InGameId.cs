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
        [SerializeField] private Int32 _mask;

        public bool Contains(InGameIdValue value)
        {
            if (!MaskContains(value.Value))
                return false;

            return ContainsBit(value.Value) == !value.IsInverse;
        }

        public bool Contains(InGameId value)
        {
            // 1 - we verify that the field target by the given value
            // are set in the current objet by comparing the masks
            if (!MaskContains(value._mask))
                return false;

            // 2 - we compare the bit differences
            int bitDifference = _id ^ value._id;

            // 3 - we keep only the differences on the targeted field
            int filteredValue = bitDifference & value._mask;

            // if no difference, then it's True, the object contains the value,
            // otherwise, it's false
            return filteredValue == 0;
        }

        public bool MaskContains(int mask)
        {
            // we check if we add the mask if the idMask is increase or not 
            return (mask | _mask) == _mask;
        }

        public void SetMask(int newMask)
        {
            _id &= newMask;
            _mask = newMask;
        }

        public void Set(InGameIdValue value, bool state)
        {
            SetBit(value.Value, state);
        }

        public void Remove(InGameIdValue value)
        {
            SetBitToFalse(value.Value);

            RemoveToMask(value.Value);
        }

        private void SetBit(int value, bool state)
        {
            if (state)
            {
                SetBitToTrue(value);
            }
            else
            {
                SetBitToFalse(value);
            }

            AddToMask(value);
        }

        private bool ContainsBit(int bits)
        {
            return (_id & bits) == bits;
        }

        private void SetBitToTrue(int bits)
        {
            _id |= bits;
        }

        private void SetBitToFalse(int bits)
        {
            _id &= (~bits);
        }

        private void AddToMask(int bits)
        {
            _mask |= bits;
        }

        private void RemoveToMask(int bits)
        {
            _mask &= (~bits);
        }

        public int GetId()
        {
            return _id;
        }

        public int GetMask()
        {
            return _mask;
        }

        /// <summary>
        /// Return all objects which have the given value from the given list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">CAREFUL : these object has to have a "InGameIdComponent"</param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<T> Filter<T>(List<T> list, InGameId value, bool checkParent = false)
            where T : MonoBehaviour
        {
            // copy the list to not affect the old
            List<T> filtered = new List<T>(list);

            for (int i = 0; i < filtered.Count; i++)
            {
                // if the object doesnt have the given value
                if (checkParent)
                {
                    if (!filtered[i].GetComponentInParent<InGameIdComponent>().Id.Contains(value))
                    {
                        // remove it
                        filtered.RemoveAt(i);
                        i--;
                    }
                }
                else
                {
                    if (!filtered[i].GetComponent<InGameIdComponent>().Id.Contains(value))
                    {
                        // remove it
                        filtered.RemoveAt(i);
                        i--;
                    }
                }
            }

            return filtered;
        }
    }
}

// -1 -------------------- 1
//             |              Norme
//                         |  Max = 30
//                 |          Actual = 5 / 30 
// 5      Norme : 0 (=> offset = 5)
//   

// StartRate = the value setted by the user
// WanderRate = the actual rate

// WanderOrientation = The actual orientation
// MaxOrientation = The max orientation (bounds)

// wanderRate = (- WanderOrientation) / MaxOrientation) * WanderStart
