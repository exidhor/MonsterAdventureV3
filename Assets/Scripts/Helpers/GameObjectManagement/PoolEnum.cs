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
        public class PoolEnum<T>
            where T : MonoBehaviour
        {
            public T Current
            {
                get { return _pool.Resources[_index].GameObject.GetComponent<T>(); }
            }

            private Pool _pool;

            private int _index;

            public PoolEnum(Pool pool)
            {
                _pool = pool;
                _index = 0;
            }

            public bool IsAtTheEnd()
            {
                return _index >= _pool.Resources.Count - 1;
            }


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
