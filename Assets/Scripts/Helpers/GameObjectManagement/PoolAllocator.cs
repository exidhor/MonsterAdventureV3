using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class PoolAllocator : MonoSingleton<PoolAllocator>
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

        public void AddPoolRequest(PoolRequest poolRequest)
        {
            if (ToResolve.Count >= PoolRequestCapacity)
            {
                Debug.Log("To many request pool stored !");
            }
            else
            {
                ToResolve.Enqueue(poolRequest);
                _numberOfActionToResolve += poolRequest.Size;
            }
        }

        public void DoInstancePoolRequest(PoolRequest poolRequest)
        {
            ResolvePoolRequest(poolRequest);
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
            int numberOfActionToResolveDuringThisFrame = GetNumberActionToResolve();

            while (ToResolve.Count > 0 && currentNumberOfResolvedAction < numberOfActionToResolveDuringThisFrame)
            {
                PoolRequest currentPoolRequest = ToResolve.Peek();

                int numberOfActionMissing = currentPoolRequest.PoolObjects.Count - _indexInCurrentRequest;

                if (numberOfActionMissing + currentNumberOfResolvedAction <= numberOfActionToResolveDuringThisFrame)
                {
                    // resolve the action
                    currentNumberOfResolvedAction += ResolvePoolRequest(currentPoolRequest, _indexInCurrentRequest, currentPoolRequest.PoolObjects.Count - 1);

                    // actualize counters
                    _indexInCurrentRequest = 0;

                    // delete the request
                    ToResolve.Dequeue();
                }
                else
                {
                    // compute the number of actions
                    numberOfActionMissing = numberOfActionToResolveDuringThisFrame - currentNumberOfResolvedAction;
                    int endIndex = numberOfActionMissing + _indexInCurrentRequest;

                    // resolve the action
                    currentNumberOfResolvedAction += ResolvePoolRequest(currentPoolRequest, _indexInCurrentRequest, endIndex);

                    // actualize counters
                    _indexInCurrentRequest = endIndex + 1;
                }
            }

            _numberOfActionToResolve -= currentNumberOfResolvedAction;

            if (_numberOfActionToResolve < 0)
            {
                Debug.LogError("Number of action negative !");
            }
        }

        private int ResolvePoolRequest(PoolRequest poolRequest)
        {
            return ResolvePoolRequest(poolRequest, 0, poolRequest.Size - 1);
        }

        private int ResolvePoolRequest(PoolRequest poolRequest, int startIndex, int endIndex)
        {
            switch (poolRequest.Action)
            {
                case PoolRequestAction.Allocate:
                        InstanciatePoolObjects(poolRequest.PoolObjects, startIndex, endIndex);
                    break;

                case PoolRequestAction.Free:
                        ReleasePoolObjects(poolRequest.PoolObjects, startIndex, endIndex);
                    break;
            }

            return endIndex - startIndex;
        }

        private void InstanciatePoolObjects(List<PoolObject> poolObjects, int startIndex, int endIndex)
        {
            if (endIndex >= poolObjects.Count)
            {
                Debug.Log("error on endIndex : " + endIndex + " >= Count : " + poolObjects.Count);
            }

            for (int i = startIndex; i <= endIndex; i++)
            {
                poolObjects[i].Instantiate();
            }
        }

        private void ReleasePoolObjects(List<PoolObject> poolObjects, int startIndex, int endIndex)
        {
            if (endIndex >= poolObjects.Count)
            {
                Debug.Log("error on endIndex : " + endIndex + " >= Count : " + poolObjects.Count);
            }

            for (int i = startIndex; i <= endIndex; i++)
            {
                poolObjects[i].Release();
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