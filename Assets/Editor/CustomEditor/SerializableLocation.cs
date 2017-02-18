using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{


    public static class SerializableLocation
    {
        public static Location DisplayOnGUI(string label, Location location)
        {
            if (location == null)
                return null;

            LocationType locationType = location.GetLocationType();

            locationType = DisplayLocationType(locationType, ref location);

            switch (locationType)
            {
                case LocationType.StationayLocation:
                    DisplayStationaryLocation((StationaryLocation) location, label);
                    break;

                case LocationType.TransformLocation:
                    DisplayTranformLocation((TransformLocation) location, label);
                    break;
            }

            return location;
        }

        private static LocationType DisplayLocationType(LocationType locationType, ref Location location)
        {
            LocationType newLocationType = (LocationType)EditorGUILayout.EnumPopup("Location type", locationType);

            if (locationType != newLocationType)
            {
                location = ConstructLocation(newLocationType);
            }

            return newLocationType;
        }

        private static Location ConstructLocation(LocationType type)
        {
            switch (type)
            {
                case LocationType.StationayLocation:
                    return new StationaryLocation(0, 0);

                case LocationType.TransformLocation:
                    return new TransformLocation();

                default:
                    return null;
            }
        }

        private static void DisplayStationaryLocation(StationaryLocation stationaryLocation, string label)
        {
            stationaryLocation.SetPosition(EditorGUILayout.Vector2Field(label, stationaryLocation.GetPosition()));
        }

        private static void DisplayTranformLocation(TransformLocation transformLocation, string label)
        {
            transformLocation.SetTransform((Transform) EditorGUILayout.ObjectField(label, transformLocation.GetTransform(), typeof(Transform), true));
        }
    }
}
