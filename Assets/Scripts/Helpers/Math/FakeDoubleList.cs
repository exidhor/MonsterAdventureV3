using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace MonsterAdventure
{
    /// <summary>
    /// Each line HAS TO SHARE the same length, ie it need to be a square grid.
    /// </summary>
    /// <typeparam name="T">The type of the List</typeparam>
    public class FakeDoubleEntryList<T>
    {
        public List<T> singleEntryList;

        public uint lineSize;

        /// <summary>
        /// todo
        /// </summary>
        /// <param name="lineSize"></param>
        public FakeDoubleEntryList(uint lineSize)
            : this(new List<T>(), lineSize)
        {
            // nothing
        }

        /// <summary>
        /// todo
        /// </summary>
        /// <param name="singleEntryList"></param>
        /// <param name="lineSize"></param>
        public FakeDoubleEntryList(List<T> singleEntryList, uint lineSize)
        {
            this.singleEntryList = singleEntryList;

            this.lineSize = lineSize;
        }

        /// <summary>
        /// todo
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public T GetElement(int x, int y)
        {
            return singleEntryList[(int)(y *lineSize) + x];
        }
    }
}
