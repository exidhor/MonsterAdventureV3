using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    public class PerceptionComponent : MonoBehaviour
    {
        public Kinematic Kinematic;

        public Arc VisionDetection;
        public Circle AutoDetection;

        private CircleCollider2D _circleCollider;

        [SerializeField] private List<Kinematic> _collidedObjects;
        [SerializeField] private List<Kinematic> _perceptibleObjects;

        //private List<Kinematic> _collidedBuffer;
        //private List<Kinematic> _perceptibleBuffer;

        void Awake()
        {
            _collidedObjects = new List<Kinematic>();
            _perceptibleObjects = new List<Kinematic>();

            //_collidedBuffer = new List<Kinematic>();
            //_perceptibleBuffer = new List<Kinematic>();

            _circleCollider = GetComponent<CircleCollider2D>();

            if (Kinematic == null)
            {
                Debug.LogError("No kinematic set for IntelligenceComponent on " + gameObject.name);
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
            VisionDetection.Center = Kinematic.GetPosition();
            VisionDetection.AngleDirection = Kinematic.OrientationInDegree;

            AutoDetection.Center = Kinematic.GetPosition();
        }

        private void FilterWithAutoDetection()
        {
            for (int i = 0; i < _collidedObjects.Count; i++)
            {
                if (AutoDetection.IsInside(_collidedObjects[i].GetPosition()))
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
                if (VisionDetection.IsInto(_collidedObjects[i].GetPosition()))
                {
                    AddToPerceptibleObjets(_collidedObjects[i], i);
                    i--;
                }
            }
        }

        private void AddToPerceptibleObjets(Kinematic kinematic, int collidedIndex)
        {
            _collidedObjects.RemoveAt(collidedIndex);
            _perceptibleObjects.Add(kinematic);
        }

        //private void OnCollisionStay2D(Collision2D collision)
        //{
        //    Kinematic Collidedkinematic = collision.gameObject.GetComponent<Kinematic>();

        //    if (Collidedkinematic != null)
        //    {
        //        _collidedObjects.Add(Collidedkinematic);
        //    }
        //}

        //private void OnTriggerEnter2D(Collider2D collider)
        //{
        //    Kinematic Collidedkinematic = collider.gameObject.GetComponent<Kinematic>();

        //    if (Collidedkinematic != null)
        //    {
        //        _collidedObjects.Add(Collidedkinematic);
        //    }
        //}

        //private void OnTriggerExit2D(Collider2D collider)
        //{
        //    Kinematic Collidedkinematic = collider.gameObject.GetComponent<Kinematic>();

        //    if (Collidedkinematic != null)
        //    {
        //        _collidedObjects.Remove(Collidedkinematic);
        //    }
        //}

        private void OnTriggerStay2D(Collider2D collider)
        {
            Kinematic Collidedkinematic = collider.gameObject.GetComponent<Kinematic>();

            if (Collidedkinematic != null)
            {
                _collidedObjects.Add(Collidedkinematic);
            }
        }

        public List<Kinematic> GetPerceptibles()
        {
            return _perceptibleObjects;
        }
    }
}