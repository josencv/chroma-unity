using System;
using System.Collections.Generic;

namespace Chroma.Core.Infrastructure.StateMachines
{
    public abstract class State
    {
        public string Name { get; }
        public List<StateTransition> Transitions { get; } = new List<StateTransition>();

        public State(string name)
        {
            this.Name = name;
        }

        public virtual void AddTransition(StateTransition transition)
        {
            if(transition.To == this)
            {
                throw new ArgumentException("cannot add a transition that points to itself");
            }

            this.Transitions.Add(transition);
        }

        /// <summary>
        /// Executed each time the state is entered
        /// </summary>
        public abstract void OnEnter();

        /// <summary>
        /// Executed each time the state exits
        /// </summary>
        public abstract void OnExit();

        /// <summary>
        /// Executed each game tick
        /// </summary>
        public abstract void Tick(float deltaTime);

        /// <summary>
        /// Evaluates transitions conditions until one is fulfilled, otherwise
        /// returns null
        /// </summary>
        /// <param name="blackboard"></param>
        /// <returns></returns>
        public StateTransition EvaluateTransitions(Blackboard blackboard)
        {
            foreach(StateTransition transition in this.Transitions)
            {
                if(transition.AreTransitionConditionsMet(blackboard))
                {
                    return transition;
                }
            }

            return null;
        }
    }
}
