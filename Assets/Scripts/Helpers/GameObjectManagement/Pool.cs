using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    /// <summary>
    /// Store copies of a GameObject model
    /// </summary>
    public partial class Pool : MonoBehaviour
    {
        // We place the object at this position
        // to avoid unnecessary collision event
        // (because Unity never disable colliders)
        public static Vector2 StoredPosition = new Vector2(1000000, 1000000);

        public List<Resource> Resources;

        private GameObject _model;

        private bool _isStatic;

        /// by how many the pool is increase if there is not enough place 
        private int _expandSize;

        private int _firstFreeResource;
        private int _lastBusyResource;

        /// <summary>
        /// Construct the pool and initialize it
        /// </summary>
        /// <param name="model">The model for all the instances</param>
        /// <param name="isStatic"></param>
        /// <param name="expandSize">by how many the pool is increase if there is not enough place </param>
        public void Construct(GameObject model, bool isStatic, uint expandSize)
        {
            _model = model;
            _isStatic = isStatic;
            _expandSize = (int) expandSize;

            name = "Pool (" + _model.name + ")";

            Resources = new List<Resource>();

            _firstFreeResource = 0;
            _lastBusyResource = 0;
        }

        /// <summary>
        /// Resize the pool
        /// </summary>
        /// <param name="newSize">The new number of instance we want</param>
        public void SetSize(uint newSize)
        {
            if (newSize < Resources.Count)
            {
                Resources.RemoveRange((int) newSize - 1, Resources.Count - (int) newSize);
            }
            else
            {
                Resources.Capacity = (int) newSize;

                ExpandSize((int) newSize - Resources.Count);
            }
        }

        /// <summary>
        /// Expand the array by creating new instance
        /// </summary>
        /// <param name="numberOfElementsToAdd"></param>
        private void ExpandSize(int numberOfElementsToAdd)
        {
            for (int i = 0; i < numberOfElementsToAdd; i++)
            {
                Resources.Add(CreateResource());
            }
        }

        /// <summary>
        /// Configure a pool object to be linked to this pool
        /// </summary>
        /// <param name="poolObject"></param>
        /// <param name="go">GameObject</param>
        /// <param name="PoolIndex">The array index in the pool</param>
        private void SetPoolObject(PoolObject poolObject, GameObject go, int PoolIndex)
        {
            poolObject.GameObject = go;
            poolObject.IndexInPool = PoolIndex;
        }

        /// <summary>
        /// Return a reference to an free instance and set it to "busy"
        /// </summary>
        /// <param name="poolObjectDest">The container where the reference will be placed</param>
        public void GetFreeResource(PoolObject poolObjectDest)
        {
            int i;
            for (i = _firstFreeResource; i < Resources.Count; i++)
            {
                if (!Resources[i].IsUsed)
                {
                    SetResourceState(true, i);
                    SetPoolObject(poolObjectDest, Resources[i].GameObject, i);
                    return;
                }
            }

            // we need to create new Resources if there isnt enough place
            ExpandSize(_expandSize);

            SetResourceState(true, i);
            SetPoolObject(poolObjectDest, Resources[i].GameObject, i);
        }

        /// <summary>
        /// Actualize the state of a resource
        /// </summary>
        /// <param name="isUsed">The state</param>
        /// <param name="index">The index of the resource to set</param>
        private void SetResourceState(bool isUsed, int index)
        {
            if (isUsed)
            {
                Resources[index].IsUsed = true;
                Resources[index].GameObject.SetActive(true);

                _firstFreeResource = index + 1;

                if (_lastBusyResource < index)
                {
                    _lastBusyResource = index;
                }
            }
            else
            {
                Resources[index].IsUsed = false;
                Resources[index].GameObject.SetActive(false);
                Resources[index].GameObject.transform.position = StoredPosition;

                if (_firstFreeResource > index)
                {
                    _firstFreeResource = index;
                }
            }
        }

        /// <summary>
        /// Try to release the resource given
        /// </summary>
        /// <param name="poolObject">The PoolObject which contains the resource</param>
        public void ReleaseResource(PoolObject poolObject)
        {
            if (poolObject.IndexInPool < 0 || poolObject.IndexInPool >= Resources.Count)
            {
                Debug.LogError("bad index : " + poolObject.IndexInPool);
            }

            if (Resources[poolObject.IndexInPool].GameObject == poolObject.GameObject)
            {
                if (Resources[poolObject.IndexInPool].IsUsed)
                {
                    SetResourceState(false, poolObject.IndexInPool);

                    poolObject.GameObject = null;
                    poolObject.IndexInPool = -1;
                }
                else
                {
                    Debug.LogError("Try to destroy an unused object in the " + name + ".\n"
                        + "It may be an error in the poolInstanceId (" + poolObject.IndexInPool + ")");
                }
            }
            else
            {
                Debug.LogError("Try to destroy a different object in the " + name + ".\n"
                               + "It may be an error in the poolInstanceId (" + poolObject.IndexInPool + ")");
            }
        }

        /// <summary>
        /// Create a new resource into the pool
        /// </summary>
        /// <returns></returns>
        private Resource CreateResource()
        {
            GameObject newGameObject = Instantiate<GameObject>(_model);
            newGameObject.transform.parent = this.gameObject.transform;
            newGameObject.name = _model.name + " " + Resources.Count;

            newGameObject.SetActive(false);

            newGameObject.transform.position = StoredPosition;

            return new Resource(newGameObject);
        }

        /// <summary>
        /// Create an enumator to iterate into the Pool
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public PoolEnum<T> GetEnumerator<T>()
            where T : MonoBehaviour
        {
            return new PoolEnum<T>(this);
        }
    }
}