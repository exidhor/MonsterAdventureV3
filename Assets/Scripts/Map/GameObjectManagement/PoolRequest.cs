using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure
{
    public enum PoolRequestAction
    {
        Free,
        Allocate
    }

    [Serializable]
    public class PoolRequest
    {
        public List<TracedObject> TracedObjects;
        public PoolRequestAction Action;

        public PoolRequest(List<TracedObject> tracedObjects, PoolRequestAction action)
        {
            TracedObjects = tracedObjects;
            Action = action;
        }

        public override string ToString()
        {
            return "PoolRequest (Count : " + TracedObjects.Count + ", Action : " + Action + ")";
        }
    }
}
