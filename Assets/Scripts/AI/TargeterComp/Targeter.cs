using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    [RequireComponent(typeof(PerceptionComponent))]
    public class Targeter : MonoBehaviour
    {
        public List<GoalEntry> Goals;

        private PerceptionComponent _perceptionComp;

        private List<GoalEntry> _activeGoals;

        void Start()
        {
            _perceptionComp = GetComponent<PerceptionComponent>();

            _activeGoals = new List<GoalEntry>();
        }

        public void Actualize()
        {
            // Verify if the goals are well ordoned
            OrderGoals();

            // Clear the old active list 
            _activeGoals.Clear();

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
            float totalWeight = 0;

            for (int i = 0; i < Goals.Count && totalWeight < 1; i++)
            {
                Goals[i].TrackedTargets = InGameId.Filter<Kinematic>(perceptibles, Goals[i].TargetValue);

                if (Goals[i].TrackedTargets.Count > 0)
                {
                    totalWeight += ActivateGoal(Goals[i]);
                }
            }
        }

        private float ActivateGoal(GoalEntry goal)
        {
            _activeGoals.Add(goal);

            return goal.Weight;
        }
    }
}
