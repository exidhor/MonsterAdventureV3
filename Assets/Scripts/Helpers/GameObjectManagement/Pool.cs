using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public partial class Pool : MonoBehaviour
    {
        public List<Resource> Resources;

        private GameObject _model;
        private bool _isStatic;
        private int _expandSize;

        private int _firstFreeResource;
        private int _lastBusyResource;

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

        private void ExpandSize(int numberOfElementsToAdd)
        {
            for (int i = 0; i < numberOfElementsToAdd; i++)
            {
                Resources.Add(CreateResource());
            }
        }

        private void SetPoolObject(PoolObject poolObject, GameObject go, int PoolIndex)
        {
            poolObject.GameObject = go;
            poolObject.IndexInPool = PoolIndex;
        }

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

            // we need to create new Resources
            ExpandSize(_expandSize);

            SetResourceState(true, i);
            SetPoolObject(poolObjectDest, Resources[i].GameObject, i);
        }

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

                if (_firstFreeResource > index)
                {
                    _firstFreeResource = index;
                }
            }
        }

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

        private Resource CreateResource()
        {
            GameObject newGameObject = Instantiate<GameObject>(_model);
            newGameObject.transform.parent = this.gameObject.transform;
            newGameObject.name = _model.name + " " + Resources.Count;

            newGameObject.SetActive(false);

            return new Resource(newGameObject);
        }

        public PoolEnum<T> GetEnumerator<T>()
            where T : MonoBehaviour
        {
            return new PoolEnum<T>(this);
        }
    }
}