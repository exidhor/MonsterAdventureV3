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
        public Trace Trace;
        public LifeTrace LifeTrace;

        public bool IsInstanciated
        {
            get { return GameObject != null; }
        }

        public Vector2 Position
        {
            get { return Trace.Position; }
            set { SetPosition(value); }
        }

        private float _lastSaveTime;

        public TracedObject(Trace trace, LifeTrace lifeTrace, Pool pool, float time)
            : base(pool)
        {
            Trace = trace;
            LifeTrace = lifeTrace;

            _lastSaveTime = time;
        }

        public override void Instantiate(float time)
        {
            Debug.Log("Instantiate");

            base.Instantiate(time);

            GameObject.transform.position = Position;

            LifeComponent lifeComponent = GameObject.GetComponent<LifeComponent>();

            if (lifeComponent != null)
            {
                LifeTrace.Activate(time - _lastSaveTime, lifeComponent);
            }
        }

        public override void Release(float time)
        {
            LifeComponent lifeComponent = GameObject.GetComponent<LifeComponent>();

            if (lifeComponent != null)
            {
                LifeTrace.Disable(lifeComponent);
            }

            _lastSaveTime = time;

            base.Release(time);
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