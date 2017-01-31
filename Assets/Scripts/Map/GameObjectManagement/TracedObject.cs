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
            get
            {
                return GameObject != null;
            }
        }

        public TracedObject(Trace trace)
        {
            Trace = trace;
            GameObject = null;
        }

        public void Instanciate(GameObject gameObject)
        {
            GameObject = gameObject;
        }

        public void Release()
        {
            GameObject = null;
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
    }
}