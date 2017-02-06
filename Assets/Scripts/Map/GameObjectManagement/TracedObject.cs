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
        public PooledObject PooledObject;

        public Trace Trace;

        public bool IsInstanciated
        {
            get { return PooledObject.GameObject != null; }
        }

        public Vector2 Position
        {
            get { return Trace.Position; }
            set { SetPosition(value); }
        }

        private Pool _pool;

        public TracedObject(Trace trace, Pool pool)
        {
            Trace = trace;
            PooledObject = new PooledObject();
            _pool = pool;
        }

        public void Instantiate()
        {
            _pool.GetFreeResource(ref PooledObject);
            PooledObject.GameObject.transform.position = Position;
        }

        public void Release()
        {
            _pool.ReleaseResource(ref PooledObject);
        }

        public override string ToString()
        {
            if (IsInstanciated)
            {
                return "TracedObject : [name : " + PooledObject.GameObject.name + " ] [ instanceId : "
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
                PooledObject.GameObject.transform.position = position;
            }
        }
    }
}