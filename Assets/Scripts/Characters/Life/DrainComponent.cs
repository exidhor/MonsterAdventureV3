using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using MonsterAdventure.AI;
using UnityEngine;

namespace MonsterAdventure
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class DrainComponent : MonoBehaviour
    {
        public Transform DrainEnd;
        public float DrainForce;
        public float BubbleSpeed;
        public InGameId TargetId;

        public float ReceivedLife;

        private CircleCollider2D _range;
        //private List<Presence> _collidedObjects;

        private List<DrainConnection> _drainConnections;

        void Awake()
        {
            _range = GetComponent<CircleCollider2D>();

            //_collidedObjects = new List<Presence>();
            _drainConnections = new List<DrainConnection>();
        }

        void Update()
        {
            //List<Presence> filteredList = FilterCollidedObject();

            //Drain(filteredList);
            Drain(Time.deltaTime);
        }

        //private List<Presence> FilterCollidedObject()
        //{
        //   return InGameId.Filter<Presence>(_collidedObjects, TargetId, true);
        //}

        private void Drain(float deltaTime)
        {
            for (int i = 0; i < _drainConnections.Count; i++)
            {
                if (!_drainConnections[i].TargetIsAlive())
                {
                    _drainConnections.RemoveAt(i);
                    i--;
                    continue;
                }

                _drainConnections[i].Actualize(deltaTime);

                if (_drainConnections[i].IsReadyToSpawn())
                {
                   _drainConnections[i].Spawn();     
                }
            }
        }

        //private void Drain(List<Presence> filteredList)
        //{
        //    for (int i = 0; i < filteredList.Count; i++)
        //    {
        //        LifeComponent lifeComponent = filteredList[i].GetComponentInParent<LifeComponent>();

        //        if (lifeComponent != null && lifeComponent.IsAlive)
        //        {
        //           MakeDrainConnection(lifeComponent);     
        //        }
        //    }
        //}

        //private void MakeDrainConnection(LifeComponent lifeComponent)
        //{
        //    DrainEngine.Instance.ConstructSoulBubble(lifeComponent.AbsorptionPoint.position, 
        //        new TransformLocation(DrainEnd),
        //        BubbleSpeed,
        //        lifeComponent.ExtractSoulBubble());
        //}

        //void OnTriggerStay2D(Collider2D collider)
        //{
        //    Presence CollidedPresence = collider.gameObject.GetComponent<Presence>();

        //    if (CollidedPresence != null)
        //    {
        //        _collidedObjects.Add(CollidedPresence);
        //    }
        //}

        void OnTriggerEnter2D(Collider2D collider)
        {
            TryToConnect(collider);
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            TryToDisconnect(collider);
        }

        private void TryToConnect(Collider2D collider)
        {
            Presence collidedPresence = collider.gameObject.GetComponent<Presence>();

            if (collidedPresence != null && MatchedTarget(collidedPresence))
            {
                CreateConnection(collidedPresence.GetComponent<LifeComponent>());
            }
        }

        private bool MatchedTarget(Presence presence)
        {
            LifeComponent lifeComponent = presence.GetComponentInParent<LifeComponent>();

            return lifeComponent != null && lifeComponent.IsAlive;
        }

        private void CreateConnection(LifeComponent lifeComponent)
        {
            _drainConnections.Add(new DrainConnection(lifeComponent, DrainEnd, BubbleSpeed, DrainForce));
        }

        private void TryToDisconnect(Collider2D collider)
        {
            Presence collidedPresence = collider.gameObject.GetComponent<Presence>();

            if (collidedPresence != null && MatchedTarget(collidedPresence))
            {
                int index = FindIndexInDrainConnections(collidedPresence.GetComponent<LifeComponent>());

                if (index > 0)
                {
                    _drainConnections.RemoveAt(index);
                }
            }
        }

        private int FindIndexInDrainConnections(LifeComponent lifeComponent)
        {
            for (int i = 0; i < _drainConnections.Count; i++)
            {
                if (_drainConnections[i].LifeComponent == lifeComponent)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
