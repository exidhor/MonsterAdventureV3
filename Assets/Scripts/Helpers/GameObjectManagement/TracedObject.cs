using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    [Serializable]
    public class TracedObject : PoolObject
    {
        //public PoolObject PoolObject;

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

        //private Pool _pool;

        public TracedObject(Trace trace, Pool pool)
            : base(pool)
        {
            Trace = trace;
        }

        public override void Instantiate()
        {
            base.Instantiate();

            GameObject.transform.position = Position;
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