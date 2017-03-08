using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public partial class Pool : MonoBehaviour
    {
        /// <summary>
        /// An Enumerator to iterate into Pools
        /// </summary>
        /// <typeparam name="T">The type of Component</typeparam>
        public class PoolEnum<T>
            where T : MonoBehaviour
        {
            /// <summary>
            /// The Current Object
            /// </summary>
            public T Current
            {
                get { return _pool.Resources[_index].GameObject.GetComponent<T>(); }
            }

            private Pool _pool;

            private int _index;

            /// <summary>
            /// Construct the Enumerator from a Pool
            /// </summary>
            /// <param name="pool"></param>
            public PoolEnum(Pool pool)
            {
                _pool = pool;
                _index = 0;
            }

            /// <summary>
            /// Check is the Enumerator is at the last index
            /// </summary>
            /// <returns>True if the Enumerator is at the last index,
            /// False otherwise</returns>
            public bool IsAtTheEnd()
            {
                return _index >= _pool.Resources.Count - 1;
            }

            /// <summary>
            /// Move the Enumerator to the next Object into the Pool
            /// </summary>
            public void Next()
            {
                if (!IsAtTheEnd())
                {
                    _index++;
                }
            }
        }
    }
}
