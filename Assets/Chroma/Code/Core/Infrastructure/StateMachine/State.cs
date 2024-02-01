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
        /// 
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
