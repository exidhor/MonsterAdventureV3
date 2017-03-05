using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class Sector : MonoBehaviour
    {
        public Rect Bounds
        {
            get { return _bounds; }
        }

        public List<TracedObject> TracedObjects;

        private Rect _bounds;

        private Coords _coords;

        private bool _isVisible;

        public void Construct(Rect bounds, Coords coords)
        {
            _bounds = bounds;
            transform.position = _bounds.position;
            _coords = coords;

            name = "Sector (" + coords.abs + ", " + coords.ord + ")";

            TracedObjects = new List<TracedObject>();
        }

        public void AddTracedObject(TracedObject tracedObject)
        {
            TracedObjects.Add(tracedObject);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(_bounds.center, _bounds.size);
        }

        public void SetIsVisible(bool isVisible)
        {
            if (_isVisible != isVisible)
            {
                _isVisible = isVisible;

                if (isVisible)
                {
                    EnableTracedObjects();
                }
                else
                {
                    DisableTracedObjects();
                }
            }
        }

        // todo : trouver comment gérer les PoolObject et les TracedObject 
        // notamment dans les IAs
        
        private void EnableTracedObjects()
        {
            //Debug.Log("Instanciate Sector " + _coords);
            PoolAllocator.Instance.AddPoolRequest(ConstructPoolRequest(PoolRequestAction.Allocate));
        }

        private void DisableTracedObjects()
        {
            //Debug.Log("Release Sector " + _coords);
            PoolAllocator.Instance.AddPoolRequest(ConstructPoolRequest(PoolRequestAction.Free));
        }

        private PoolRequest ConstructPoolRequest(PoolRequestAction action)
        {
            List<PoolObject> poolObjects = new List<PoolObject>(TracedObjects.Count);

            for (int i = 0; i < TracedObjects.Count; i++)
            {
                poolObjects.Add(TracedObjects[i]);
            }

            return new PoolRequest(poolObjects, action);
        }

        public Coords GetCoords()
        {
            return _coords;
        }
    }
}