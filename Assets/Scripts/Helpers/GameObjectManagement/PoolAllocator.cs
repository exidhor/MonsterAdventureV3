using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    /// <summary>
    /// The EntryPoint to request memory action into the different Pool.
    /// Its a MonoSingleton ! Two instances of this class doesnt make sens 
    /// and can lead to unknown behavior
    /// </summary>
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

        /// <summary>
        /// Do the allocations
        /// </summary>
        private void Awake()
        {
            // set the capacity
            ToResolve = new Queue<PoolRequest>((int) PoolRequestCapacity);
        }

        /// <summary>
        /// Init the Object
        /// </summary>
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

        /// <summary>
        /// Add a PoolRequest to the current stack, and resolve it 
        /// when we could, depending of the number of request asked before.
        /// It means that the request will be resolved with delay.
        /// If you need instant resolve, use "ResolveInstantPoolRequest" instead
        /// </summary>
        /// <param name="poolRequest">The PoolRequest to resolve with delay</param>
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

        /// <summary>
        /// Resolve a PoolRequest instantaneous.
        /// It means that the PoolRequest is not store in the stack
        /// but resolved into this method.
        /// If the PoolRequest hasn't priority or you want to delay
        /// the allocation, use "AddPoolRequest" instead.
        /// </summary>
        /// <param name="poolRequest">The PoolRequest to resolve without delay</param>
        public void ResolveInstantPoolRequest(PoolRequest poolRequest)
        {
            ResolvePoolRequest(poolRequest);
        }

        /// <summary>
        /// Stop the allocation coroutine
        /// </summary>
        private void StopAllocationCoroutine()
        {
            _continueCoroutine = false;
        }

        /// <summary>
        /// The Coroutine Allocation.
        /// It seems to be a little less performant than the Update way.
        /// Then prefer the "Update" way.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CoroutineAllocation()
        {
            while (_continueCoroutine)
            {
                Resolve();
                yield return null;
            }
        }

        /// <summary>
        /// Resolve the given number of action stored into the PoolRequest stack.
        /// The algorithm is quite simple :
        ///     It iterates into the PoolRequest stack, and for each PoolRequest, do :
        ///         - If the PoolRequest contains less actions than the current actions
        ///             missed for this update, we resolve the entirely PoolRequest and continue
        ///         - If not we resolve only the missed number of action and store the
        ///             last action index done to resolve it at the next update.
        /// </summary>
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

        /// <summary>
        ///         /// Find the right action for the current PoolRequest.
        /// </summary>
        /// <param name="poolRequest">The PoolRequest which contains the action to resolve</param>
        /// <returns>The number of actions resolved</returns>
        private int ResolvePoolRequest(PoolRequest poolRequest)
        {
            return ResolvePoolRequest(poolRequest, 0, poolRequest.Size - 1);
        }

        /// <summary>
        /// Find the right action for the current PoolRequest.
        /// It can be an Allocation or a Release.
        /// </summary>
        /// <param name="poolRequest">The PoolRequest which contains the action to resolve</param>
        /// <param name="startIndex">The included index to start</param>
        /// <param name="endIndex">The included index to end</param>
        /// <returns>The number of actions resolved</returns>
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

        /// <summary>
        /// Instanciate PoolObject needed by setting their state to "Busy"
        /// </summary>
        /// <param name="poolObjects">The PoolObject to instanciate</param>
        /// <param name="startIndex">The included index to start</param>
        /// <param name="endIndex">The included index to end</param>
        private void InstanciatePoolObjects(List<PoolObject> poolObjects, int startIndex, int endIndex)
        {
            if (endIndex >= poolObjects.Count)
            {
                Debug.Log("error on endIndex : " + endIndex + " >= Count : " + poolObjects.Count);
            }

            for (int i = startIndex; i <= endIndex; i++)
            {
                poolObjects[i].Instantiate(Time.time);
            }
        }

        /// <summary>
        /// Release the PoolObject by setting their state to "Free"
        /// </summary>
        /// <param name="poolObjects">PoolObject to release</param>
        /// <param name="startIndex">The included index to start</param>
        /// <param name="endIndex">The included index to end</param>
        private void ReleasePoolObjects(List<PoolObject> poolObjects, int startIndex, int endIndex)
        {
            if (endIndex >= poolObjects.Count)
            {
                Debug.Log("error on endIndex : " + endIndex + " >= Count : " + poolObjects.Count);
            }

            for (int i = startIndex; i <= endIndex; i++)
            {
                poolObjects[i].Release(Time.time);
            }
        }

        /// <summary>
        /// Return the number of allocations to resolve for this Update.
        /// To give this number, we need to compute and find the biggest number between
        /// the "MinAllocation" and the "SplitAction" (which is the numberOfAction in the
        /// stack divide by the "MaxSplitAllocations")
        /// </summary>
        /// <returns>The number of allocations to resolve for this Update</returns>
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