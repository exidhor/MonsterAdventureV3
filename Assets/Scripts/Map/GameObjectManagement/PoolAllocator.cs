using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class PoolAllocator : MonoBehaviour
    {
        public List<Resource> Resources;

        private GameObject _model;
        private bool _isStatic;
        private int _expandSize;

        private int _lastFreeResource;

        public void Construct(GameObject model, bool isStatic, uint expandSize)
        {
            _model = model;
            _isStatic = isStatic;
            _expandSize = (int) expandSize;

            name = "PoolAllocator (" + _model.name + ")";

            Resources = new List<Resource>();

            _lastFreeResource = 0;
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

        public void GetFreeResource(ref PoolObject poolObjectDest)
        {
            int i;
            for (i = _lastFreeResource; i < Resources.Count; i++)
            {
                if (!Resources[i].IsUsed)
                {
                    SetResourceState(true, i);

                    poolObjectDest.GameObject = Resources[i].GameObject;
                    poolObjectDest.PoolInstanceId = i;

                    return;
                }
            }

            // we need to create new Resources
            ExpandSize(_expandSize);

            SetResourceState(true, i);

            poolObjectDest.GameObject = Resources[i].GameObject;
            poolObjectDest.PoolInstanceId = i;
        }

        private void SetResourceState(bool isUsed, int index)
        {
            if (isUsed)
            {
                Resources[index].IsUsed = true;
                Resources[index].GameObject.SetActive(true);

                _lastFreeResource = index + 1;
            }
            else
            {
                Resources[index].IsUsed = false;
                Resources[index].GameObject.SetActive(false);

                if (_lastFreeResource > index)
                {
                    _lastFreeResource = index;
                }
            }
        }

        public void ReleaseResource(ref PoolObject poolObject)
        {
            if (Resources[poolObject.PoolInstanceId].GameObject == poolObject.GameObject)
            {
                if (Resources[poolObject.PoolInstanceId].IsUsed)
                {
                    SetResourceState(false, poolObject.PoolInstanceId);

                    poolObject.GameObject = null;
                    poolObject.PoolInstanceId = -1;
                }
                else
                {
                    Debug.LogError("Try to destroy an unused object in the " + name + ".\n"
                        + "It may be an error in the poolInstanceId (" + poolObject.PoolInstanceId + ")");
                }
            }
            else
            {
                Debug.LogError("Try to destroy a different object in the " + name + ".\n"
                               + "It may be an error in the poolInstanceId (" + poolObject.PoolInstanceId + ")");
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
    }
}