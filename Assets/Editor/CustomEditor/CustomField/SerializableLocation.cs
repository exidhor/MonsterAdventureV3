using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public class SerializableLocation
    {
        //private LocationComponent _locationComponent;

        //private LocationType _pendingLocationType;
        private EnumField _pendingLocationType;

        private TransformField _pendingTransform;
        private Vector2Field _pendingPosition;

        public SerializableLocation()
        {
            _pendingLocationType = new EnumField(LocationType.StationaryLocation);

            _pendingTransform = new TransformField(null);
            _pendingPosition = new Vector2Field(Vector2.zero);
        }

        //public SerializableLocation(LocationComponent locationComponent)
        //{
        //    Initialize(locationComponent);
        //}

        //public void Initialize(LocationComponent locationComponent)
        //{
        //    _locationComponent = locationComponent;

        //    _pendingLocationType = _locationComponent.GetLocationType();

        //    switch (_pendingLocationType)
        //    {
        //        case LocationType.StationaryLocation:
        //            SetPendingLocation(_locationComponent.GetPosition());
        //            break;

        //        case LocationType.TransformLocation:
        //            SetPendingTransform(_locationComponent.GetTransform());
        //            break;
        //    }
        //}

        //private void SetPendingLocation(Vector2 position)
        //{
        //    _pendingPosition = position;
        //}

        //private void SetPendingTransform(Transform transform)
        //{
        //    _pendingTransform = transform;
        //}

        public void DisplayOnGUI(string label)
        {
            _pendingLocationType.DisplayOnGUI("Location type");
            //= (LocationType) EditorGUILayout.EnumPopup("Location type", _pendingLocationType);

            switch ((LocationType) _pendingLocationType.Value)
            {
                case LocationType.StationaryLocation:
                    DisplayStationaryLocation(label);
                    break;

                case LocationType.TransformLocation:
                    DisplayTranformLocation(label);
                    break;
            }
        }

        public Location ConstructLocation()
        {
            switch ((LocationType)_pendingLocationType.Value)
            {
                case LocationType.TransformLocation:
                    return new TransformLocation(_pendingTransform.Value);

                default:
                    return new StationaryLocation(_pendingPosition.Value);
            }
        }

        public void Actualize(LocationComponent locationComponent)
        {
            _pendingLocationType.SetValue(locationComponent.GetLocationType());

            _pendingPosition.SetValue(locationComponent.GetPosition());
            _pendingTransform.SetValue(locationComponent.GetTransform());
        }

        //private LocationType DisplayLocationType(LocationType locationType, ref Location location)
        //{
        //    LocationType newLocationType = (LocationType)EditorGUILayout.EnumPopup("Location type", locationType);

        //    if (locationType != newLocationType)
        //    {
        //        location = ConstructLocation(newLocationType);
        //    }

        //    return newLocationType;
        //}

        //private static Location ConstructLocation(LocationType type)
        //{
        //    switch (type)
        //    {
        //        case LocationType.StationayLocation:
        //            return new StationaryLocation(0, 0);

        //        case LocationType.TransformLocation:
        //            return new TransformLocation();

        //        default:
        //            return null;
        //    }
        //}

        private void DisplayStationaryLocation(string label)
        {
            //stationaryLocation.SetPosition(EditorGUILayout.Vector2Field(label, stationaryLocation.GetPosition()));
            _pendingPosition.DisplayOnGUI(label);
        }

        private void DisplayTranformLocation(string label)
        {
            ///*transformLocation.SetTransform((Transform) EditorGUILayout.ObjectField(label, transformLocation.GetTransform*/(), typeof(Transform), true));
            _pendingTransform.DisplayOnGUI(label);
        }
    }
}
