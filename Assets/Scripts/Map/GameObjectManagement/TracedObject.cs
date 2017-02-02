using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    [Serializable]
    public class TracedObject
    {
        public GameObject GameObject;

        public Trace Trace;

        public bool IsInstanciated
        {
            get { return GameObject != null; }
        }

        public Vector2 Position
        {
            get { return Trace.Position; }
            set { SetPosition(value); }
        }

        private PoolAllocator _poolAllocator;

        public TracedObject(Trace trace, PoolAllocator poolAllocator)
        {
            Trace = trace;
            GameObject = null;
            _poolAllocator = poolAllocator;

            // tmp 
            //Instanciate();
        }

        public void Instanciate()
        {
            GameObject = _poolAllocator.GetFreeResource();
            GameObject.transform.position = Position;
        }

        public void Release()
        {
            _poolAllocator.ReleaseResource(ref GameObject);
        }

        public override string ToString()
        {
            if (IsInstanciated)
            {
                return "TracedObject : [name : " + GameObject.name + " ] [ instanceId : "
                       + Trace.InstanceID + " ] [ position : " + Trace.Position + " ]";
            }

            return "TracedObject : [NOT_INSTANCIATE] [ instanceId : "
                   + Trace.InstanceID + " ] [ position : " + Trace.Position + " ]";
        }

        private void SetPosition(Vector2 position)
        {
            Trace.Position = position;

            if (IsInstanciated)
            {
                GameObject.transform.position = position;
            }
        }
    }
}