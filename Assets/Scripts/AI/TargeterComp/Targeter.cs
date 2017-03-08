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
        [SerializeField]
        private EGoalAction _currentAction;
        [SerializeField]
        private List<Kinematic> _targets;

        public List<GoalEntry> Goals;

        private PerceptionComponent _perceptionComp;
        private SteeringComponent _steeringComp;

        //private List<GoalEntry> _activeGoals;


        void Start()
        {
            _perceptionComp = GetComponent<PerceptionComponent>();
            _steeringComp = GetComponent<SteeringComponent>();

            //_activeGoals = new List<GoalEntry>();
            _targets = new List<Kinematic>();
        }

        public void Actualize()
        {
            // Verify if the goals are well ordoned
            OrderGoals();

            // Clear the old active list 
            //_activeGoals.Clear();

            // work out with the world extracted data
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

            for (int i = 0; i < Goals.Count /*&& totalWeight < 1*/; i++)
            {
                Goals[i].TrackedTargets = InGameId.Filter<Kinematic>(perceptibles, Goals[i].TargetValue);

                if (Goals[i].TrackedTargets.Count > 0)
                {
                    //totalWeight += ActivateGoal(Goals[i]);
                    TryToActivateGoal(Goals[i]);
                }
            }
        }

        private void TryToActivateGoal(GoalEntry goal)
        {
            if (_currentAction == goal.action)
            {
                ChangeTargetSteering(goal.TrackedTargets);   
            }
            else
            {
                ConstructSteering(goal.action, goal.TrackedTargets);
            }
        }

        private void ChangeTargetSteering(List<Kinematic> targetKinematics)
        {
            Debug.Log("Change Target Steering");

            _targets = targetKinematics;

            // todo : call the SteeringComponent
        }

        private void ConstructSteering(EGoalAction action, List<Kinematic> targetKinematics)
        {
            Debug.Log("Construct Steering");

            _currentAction = action;
            _targets = targetKinematics;

            // todo : call the SteeringComponent
        }
    }
}
