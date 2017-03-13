using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure
{
    public class MappedList<Key, Obj>
    {
        public int Count
        {
            get { return _list.Count; }
        }

        public int Capacity
        {
            get { return _list.Capacity; }
            
            // we cant set the map capacity (!!!) 
            set { _list.Capacity = value; }
        }

        private List<Obj> _list;
        private Dictionary<Key, int> _map;

        private int lastIndex
        {
            get { return _list.Count - 1; }
        }

        public MappedList()
        {
            _list = new List<Obj>();
            _map = new Dictionary<Key, int>();
        }

        public void Add(Key key, Obj obj)
        {
            _list.Add(obj);
            _map.Add(key, lastIndex);
        }

        // the fastest way to access
        public Obj GetByIndex(int index)
        {
            return _list[index];
        }

        public Obj GetByKey(Key key)
        {
            return GetByIndex(GetIndex(key));
        }

        public int GetIndex(Key key)
        {
            return _map[key];
        }

        public Key GetKey(int index)
        {
            return _map.Keys.ElementAt(index);
        }

        public bool ContainsKey(Key key)
        {
            return _map.ContainsKey(key);
        }

        public void Clear()
        {
            _list.Clear();
            _map.Clear();
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
            _map.Remove(GetKey(index));
        }

        // the fastest way to remove
        public void RemoveFromKey(Key key)
        {
            int index = GetIndex(key);

            _map.Remove(key);
            _list.RemoveAt(index);
        }
    }
}