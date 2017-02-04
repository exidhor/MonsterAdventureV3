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
        public PoolObject PoolObject;

        public Trace Trace;

        public bool IsInstanciated
        {
            get { return PoolObject.GameObject != null; }
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
            PoolObject = new PoolObject();
            _poolAllocator = poolAllocator;
        }

        public void Instantiate()
        {
            _poolAllocator.GetFreeResource(ref PoolObject);
            PoolObject.GameObject.transform.position = Position;
        }

        public void Release()
        {
            _poolAllocator.ReleaseResource(ref PoolObject);
        }

        public override string ToString()
        {
            if (IsInstanciated)
            {
                return "TracedObject : [name : " + PoolObject.GameObject.name + " ] [ instanceId : "
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
                PoolObject.GameObject.transform.position = position;
            }
        }
    }
}