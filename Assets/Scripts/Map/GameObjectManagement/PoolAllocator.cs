using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class PoolAllocator : MonoBehaviour
    {
        private GameObject _model;
        private bool _isStatic;
        private int _expandSize;

        public List<Resource> Resources;

        public void Construct(GameObject model, bool isStatic, uint expandSize)
        {
            _model = model;
            _isStatic = isStatic;
            _expandSize = (int)expandSize;

            Resources = new List<Resource>();
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

                ExpandSize((int)newSize - Resources.Count);
            }
        }

        private void ExpandSize(int numberOfElementsToAdd)
        {
            for (int i = 0; i < numberOfElementsToAdd; i++)
            {
                Resources.Add(CreateResource());
            }
        }

        public GameObject GetFreeResource()
        {
            int i;
            for (i = 0; i < Resources.Count; i++)
            {
                if (!Resources[i].IsUsed)
                {
                    Resources[i].IsUsed = true;
                    return Resources[i].GameObject;
                }
            }

            // we need to create new Resources
            ExpandSize(_expandSize);

            Resources[i].IsUsed = true;

            return Resources[i].GameObject;
        }

        public void ReleaseResource(GameObject gameObject)
        {
            for (int i = 0; i < Resources.Count; i++)
            {
                if (Resources[i].GameObject == gameObject)
                {
                    Resources[i].IsUsed = false;
                }
            }

            Debug.Log("Try to destroy " + gameObject.ToString() 
                + " but it impossible to find it.");
        }

        private Resource CreateResource()
        {
            GameObject newGameObject = Instantiate<GameObject>(_model);
            newGameObject.transform.parent = this.gameObject.transform;

            newGameObject.SetActive(false);

            return new Resource(newGameObject);
        }
    }
}