using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    [RequireComponent(typeof(PerceptionComponent), typeof(SteeringComponent))]
    public class Targeter : MonoBehaviour
    {
        [SerializeField] private EBehavior _currentAction;

        public List<GoalEntry> Goals;

        private MappedList<string, List<GoalEntry>> _sortedGoals;

        private PerceptionComponent _perceptionComp;
        private SteeringComponent _steeringComp;

        void Start()
        {
            _perceptionComp = GetComponent<PerceptionComponent>();
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
            // Verify if the goals are well ordoned
            //OrderGoals();

            // work out with the world extracted data and 
            // send the active goal to the steeringComponent
            ActualizeGoals(_perceptionComp.GetPerceptibles());
        }

        private void OrderGoals()
        {
            // sort by priority descending
            Goals.Sort((x, y) => y.Priority.CompareTo(x.Priority));
        }

        private void ActualizeGoals(List<Kinematic> perceptibles)
        {
            // By priority, try to find the right behavior from world extracted data
            //float totalWeight = 0;

            //for (int i = 0; i < Goals.Count /*&& totalWeight < 1*/; i++)
            //{
            //    Goals[i].TrackedTargets = InGameId.Filter<Kinematic>(perceptibles, Goals[i].TargetValue);

            //    if (Goals[i].TrackedTargets.Count > 0)
            //    {
            //        //totalWeight += ActivateGoal(Goals[i]);
            //        TryToActivateGoal(Goals[i]);
            //        return;
            //    }
            //}

            // prepare the steeringComponent to receive the new goals
            _steeringComp.ClearBehaviors();

            for (int mappedIndex = 0; mappedIndex < _sortedGoals.Count; mappedIndex++)
            {
                List<GoalEntry> goalList = _sortedGoals.GetByIndex(mappedIndex);

                // iterate into the goals list until we reach the weight sum required (== 1)
                float totalWeight = 0;

                for (int listIndex = 0; listIndex < goalList.Count; listIndex++)
                {
                    goalList[listIndex].TrackedTargets = InGameId.Filter<Kinematic>(perceptibles, goalList[listIndex].TargetValue);

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
            //if (_currentAction == goal.behavior)
            //{
            //    ChangeTargetSteering(goal.TrackedTargets);   
            //}
            //else
            //{
            //ConstructSteering(goal.behavior, goal.TrackedTargets);

            _steeringComp.AddBehavior(goal.behavior, goal.TrackedTargets, goal.Weight);
            //}
        }

        //private void ChangeTargetSteering(List<Kinematic> targetKinematics)
        //{
        //    Debug.Log("Change Target Steering");

        //    _targets = targetKinematics;

        //    // todo : call the SteeringComponent
        //}

        //private void ConstructSteering(EBehavior behavior, List<Kinematic> targetKinematics, float weight)
        //{
        //    //Debug.Log("Construct Steering");

        //    _currentAction = behavior;

        //    _steeringComp.AddBehavior(behavior, targetKinematics, weight);
        //    //_steeringComp.SetActiveBehavior(behavior, targetKinematics, 1f);
        //}
    }
}