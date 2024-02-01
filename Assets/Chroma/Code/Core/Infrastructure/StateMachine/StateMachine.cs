using System;
using System.Collections.Generic;

namespace Chroma.Core.Infrastructure.StateMachines
{
    public class StateMachine
    {
        private Blackboard blackboard = new Blackboard();
        private bool shouldEvaluateTransitions = false;
        private State entrypoint;
        private State currentState;

        public StateMachine(State entrypoint)
        {
            this.entrypoint = entrypoint;
        }

        public void Start()
        {
            this.currentState = this.entrypoint;
            this.currentState.OnEnter();
            // when the machine starts, we need to immediately evaluate transitions
            // because we don't know the current state of the blackboard
            this.shouldEvaluateTransitions = true;
        }

        public void Tick(float deltaTime)
        {
            this.currentState.Tick(deltaTime);
            if(this.shouldEvaluateTransitions)
            {
                this.EvaluateTransitions();
                this.shouldEvaluateTransitions = false;
            }
        }

        public void RegisterBool(string name, bool initialValue = false)
        {
            this.blackboard.RegisterBool(name, initialValue);
        }

        public void RegisterFloat(string name, float initialValue = 0.0f)
        {
            this.blackboard.RegisterFloat(name, initialValue);
        }

        public void RegisterInt(string name, int initialValue)
        {
            this.blackboard.RegisterInt(name, initialValue);
        }

        public void RegisterTrigger(string name)
        {
            this.blackboard.RegisterTrigger(name);
        }

        public void UpdateBool(string name, bool value)
        {
            this.shouldEvaluateTransitions = true;
            this.blackboard.UpdateBool(name, value);
        }

        public void UpdateFloat(string name, float value)
        {
            this.shouldEvaluateTransitions = true;
            this.blackboard.UpdateFloat(name, value);
        }

        public void UpdateInt(string name, int value)
        {
            this.shouldEvaluateTransitions = true;
            this.blackboard.UpdateInt(name, value);
        }

        public void ActivateTrigger(string name)
        {
            this.shouldEvaluateTransitions = true;
            this.blackboard.ActivateTrigger(name);
        }

        private void ChangeState(State targetState)
        {
            this.currentState.OnExit();
            targetState.OnEnter();
            this.currentState = targetState;
        }

        private void EvaluateTransitions()
        {
            var visitedStates = new HashSet<int>();
            this.EvaluateTransitions(visitedStates);

        }

        private void EvaluateTransitions(HashSet<int> visitedStates)
        {
            StateTransition transition = this.currentState.EvaluateTransitions(this.blackboard);
            if(transition != null)
            {
                int stateHash = transition.To.GetHashCode();
                if(visitedStates.Contains(stateHash))
                {
                    throw new ApplicationException($"the state '{stateHash}' has been visited twice in a single evaluation tick. " +
                        "Check that there are no logical loops in your state machine");
                }

                this.ChangeState(transition.To);
                visitedStates.Add(stateHash);
                // evaluate immediately to check if the next state should also change state
                this.EvaluateTransitions(visitedStates);
            }
        }
    }
}
