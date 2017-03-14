using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class PerceptionComponent : MonoBehaviour
    {
        private Kinematic _kinematic;

        public Arc VisionDetection;
        public Circle AutoDetection;

        private CircleCollider2D _circleCollider;

        [SerializeField] private List<Presence> _collidedObjects;
        [SerializeField] private List<Presence> _perceptibleObjects;

        void Awake()
        {
            _collidedObjects = new List<Presence> ();
            _perceptibleObjects = new List<Presence>();

            _circleCollider = GetComponent<CircleCollider2D>();

            _kinematic = GetComponentInParent<Kinematic>();

            if (_kinematic == null)
            {
                Debug.LogWarning("No Kinematic set into PerceptionComponent");
            }
        }

        // has to be called from "FixedUpdate" method
        public void Actualize()
        {
            // reset the found object list
            _perceptibleObjects.Clear();

            ActualizeFromKinematic();

            FilterWithAutoDetection();
            FilterWithVisionDetection();

            // prepare for the next check
            _collidedObjects.Clear();
        }

        private void ActualizeFromKinematic()
        {
            VisionDetection.Center = _kinematic.GetPosition();
            VisionDetection.AngleDirection = _kinematic.OrientationInDegree;

            AutoDetection.Center = _kinematic.GetPosition();
        }

        private void FilterWithAutoDetection()
        {
            for (int i = 0; i < _collidedObjects.Count; i++)
            {
                if (AutoDetection.IsInside(_collidedObjects[i].Position))
                {
                    AddToPerceptibleObjets(_collidedObjects[i], i);
                    i--;
                }
            }
        }

        private void FilterWithVisionDetection()
        {
            for (int i = 0; i < _collidedObjects.Count; i++)
            {
                if (VisionDetection.IsInto(_collidedObjects[i].Position))
                {
                    AddToPerceptibleObjets(_collidedObjects[i], i);
                    i--;
                }
            }
        }

        private void AddToPerceptibleObjets(Presence presence, int collidedIndex)
        {
            _collidedObjects.RemoveAt(collidedIndex);
            _perceptibleObjects.Add(presence);
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            Presence CollidedPresence = collider.gameObject.GetComponent<Presence>();

            if (CollidedPresence != null)
            {
                _collidedObjects.Add(CollidedPresence);
            }
        }

        public List<Presence> GetPerceptibles()
        {
            return _perceptibleObjects;
        }
    }
}