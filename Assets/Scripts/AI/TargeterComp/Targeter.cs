using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    [RequireComponent(typeof(IntelligenceComponent), typeof(SteeringComponent))]
    public class Targeter : MonoBehaviour
    {
        public List<GoalEntry> Goals;

        private MappedList<string, List<GoalEntry>> _sortedGoals;

        private PerceptionComponent _perceptionComp;
        private SteeringComponent _steeringComp;

        void Start()
        {
            _perceptionComp = GetComponentInChildren<PerceptionComponent>();
            _steeringComp = GetComponent<SteeringComponent>();

            _sortedGoals = new MappedList<string, List<GoalEntry>>();

            ConstructSortedGoals();
        }

        private void ConstructSortedGoals()
        {
            // fill the mappedList
            for (int i = 0; i < Goals.Count; i++)
            {
                if (_sortedGoals.ContainsKey(Goals[i].GroupKey))
                {
                    _sortedGoals.GetByKey(Goals[i].GroupKey).Add(Goals[i]);
                }
                else
                {
                    List<GoalEntry> newList = new List<GoalEntry>();
                    newList.Add(Goals[i]);

                    _sortedGoals.Add(Goals[i].GroupKey, newList);
                }
            }

            // order each group list in the mappedList
            for (int i = 0; i < _sortedGoals.Count; i++)
            {
                _sortedGoals.GetByIndex(i).Sort((x, y) => y.Priority.CompareTo(x.Priority));
            }
        }

        public void Actualize()
        {
            // work out with the world extracted data and 
            // send the active goal to the steeringComponent
            ActualizeGoals(_perceptionComp.GetPerceptibles());
        }

        private void OrderGoals()
        {
            // sort by priority descending
            Goals.Sort((x, y) => y.Priority.CompareTo(x.Priority));
        }

        private void ActualizeGoals(List<Presence> perceptibles)
        {
            // prepare the steeringComponent to receive the new goals
            _steeringComp.ClearBehaviors();

            for (int mappedIndex = 0; mappedIndex < _sortedGoals.Count; mappedIndex++)
            {
                List<GoalEntry> goalList = _sortedGoals.GetByIndex(mappedIndex);

                // iterate into the goals list until we reach the weight sum required (== 1)
                float totalWeight = 0;

                for (int listIndex = 0; listIndex < goalList.Count; listIndex++)
                {
                    goalList[listIndex].TrackedTargets = InGameId.Filter<Presence>(perceptibles, goalList[listIndex].TargetValue, true);

                    if (goalList[listIndex].TrackedTargets.Count > 0 || goalList[listIndex].IsDefault)
                    {
                        ActivateGoal(goalList[listIndex]);

                        totalWeight += goalList[listIndex].Weight;

                        if (totalWeight >= 1)
                        {
                            break; // make end of this goal list
                        }
                    }
                }
            }
        }

        private void ActivateGoal(GoalEntry goal)
        {
            _steeringComp.AddBehavior(goal.behavior, ConvertList(goal.TrackedTargets), goal.Weight);
        }

        private List<Location> ConvertList(List<Presence> presences)
        {
            List<Location> locations = new List<Location>();

            for (int i = 0; i < presences.Count; i++)
            {
                if (presences[i].Kinematic != null)
                {
                    locations.Add(new KinematicLocation(presences[i].Kinematic));
                }
                else
                {
                    locations.Add(new TransformLocation(presences[i].transform));
                }
            }

            return locations;
        }
    }
}