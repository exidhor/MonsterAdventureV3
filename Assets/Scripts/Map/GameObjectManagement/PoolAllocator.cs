using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class PoolAllocator : MonoBehaviour
    {
        public bool UseCoroutine;

        public uint MinAllocations;
        public uint MaxSplitAllocations;
        public uint PoolRequestCapacity;

        public Queue<PoolRequest> ToResolve;
        private int _numberOfActionToResolve;
        private int _indexInCurrentRequest;

        private IEnumerator _coroutine;
        private bool _continueCoroutine;

        private void Awake()
        {
            // set the capacity
            ToResolve = new Queue<PoolRequest>((int) PoolRequestCapacity);
        }

        private void Start()
        {
            _continueCoroutine = true;
            _coroutine = CoroutineAllocation();

            _indexInCurrentRequest = 0;

            if(UseCoroutine)
                StartCoroutine(_coroutine);
        }

        private void Update()
        {
            if(!UseCoroutine)
                Resolve();
        }

        public void AddPoolRequest(List<TracedObject> tracedObjects,
            PoolRequestAction action)
        {
            if (ToResolve.Count >= PoolRequestCapacity)
            {
                Debug.Log("To many request pool stored !");
            }
            else
            {
                ToResolve.Enqueue(new PoolRequest(tracedObjects, action));
                _numberOfActionToResolve += tracedObjects.Count;
            }
        }

        private void StopAllocationCoroutine()
        {
            _continueCoroutine = false;
        }

        private IEnumerator CoroutineAllocation()
        {
            while (_continueCoroutine)
            {
                Resolve();
                yield return null;
            }
        }

        private void Resolve()
        {
            int currentNumberOfResolvedAction = 0;
            int numberOfActionToResolve = GetNumberActionToResolve();

            while (ToResolve.Count > 0 && currentNumberOfResolvedAction < numberOfActionToResolve)
            {
                PoolRequest currentPoolRequest = ToResolve.Peek();

                int numberOfActionMissing = currentPoolRequest.TracedObjects.Count - _indexInCurrentRequest;

                if (numberOfActionMissing + currentNumberOfResolvedAction <= numberOfActionToResolve)
                {
                    // resolve the action
                    ResolvePoolRequest(currentPoolRequest, _indexInCurrentRequest, currentPoolRequest.TracedObjects.Count - 1);

                    // actualize counters
                    _indexInCurrentRequest = 0;
                    currentNumberOfResolvedAction += currentPoolRequest.TracedObjects.Count;

                    // delete the request
                    ToResolve.Dequeue();
                }
                else
                {
                    // compute the number of actions
                    numberOfActionMissing = numberOfActionToResolve - currentNumberOfResolvedAction;
                    int endIndex = numberOfActionMissing + _indexInCurrentRequest;

                    // resolve the action
                    ResolvePoolRequest(currentPoolRequest, _indexInCurrentRequest, endIndex);

                    // actualize counters
                    _indexInCurrentRequest = endIndex + 1;
                    currentNumberOfResolvedAction += numberOfActionMissing;

                    // actualize the request
                    //currentPoolRequest.TracedObjects.RemoveRange(0, endIndex);
                }
            }

            _numberOfActionToResolve -= currentNumberOfResolvedAction;

        }

        private void ResolvePoolRequest(PoolRequest poolRequest, int startIndex, int endIndex)
        {
            switch (poolRequest.Action)
            {
                case PoolRequestAction.Allocate:
                        InstanciateTracedObjects(poolRequest.TracedObjects, startIndex, endIndex);
                    break;

                case PoolRequestAction.Free:
                        ReleaseTracedObjects(poolRequest.TracedObjects, startIndex, endIndex);
                    break;
            }
        }

        private void InstanciateTracedObjects(List<TracedObject> tracedObjects, int startIndex, int endIndex)
        {
            if (endIndex >= tracedObjects.Count)
            {
                Debug.Log("error on endIndex : " + endIndex + " >= Count : " + tracedObjects.Count);
            }

            for (int i = startIndex; i <= endIndex; i++)
            {
                tracedObjects[i].Instantiate();
            }
        }

        private void ReleaseTracedObjects(List<TracedObject> tracedObjects, int startIndex, int endIndex)
        {
            if (endIndex >= tracedObjects.Count)
            {
                Debug.Log("error on endIndex : " + endIndex + " >= Count : " + tracedObjects.Count);
            }

            for (int i = startIndex; i <= endIndex; i++)
            {
                tracedObjects[i].Release();
            }
        }

        private int GetNumberActionToResolve()
        {
            int splitAction = (int)(_numberOfActionToResolve / (float)MaxSplitAllocations);

            if (splitAction < MinAllocations)
            {
                return (int)MinAllocations;
            }

            return splitAction;
        }
    }
}