using System.Collections.Generic;

namespace Chroma.Core.Infrastructure.StateMachines
{
    public class StateTransition
    {
        private List<TransitionCondition> conditions;
        public State To { get; }

        public StateTransition(State to, List<TransitionCondition> conditions)
        {
            this.To = to;
            this.conditions = conditions;
        }

        public bool AreTransitionConditionsMet(Blackboard blackboard)
        {
            foreach(TransitionCondition condition in this.conditions)
            {
                if(!this.IsConditionMet(blackboard, condition))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsConditionMet(Blackboard blackboard, TransitionCondition condition)
        {
            BlackboardVariable variable = blackboard.GetVariable(condition.LeftOperandVarName);
            bool isConditionMet;

            if(condition.ConditionOperator == ConditionOperator.Greater)
            {
                isConditionMet = variable.Value > condition.RightOperandValue;
            }
            else if(condition.ConditionOperator == ConditionOperator.Less)
            {
                isConditionMet = variable.Value < condition.RightOperandValue;
            }
            else if(condition.ConditionOperator == ConditionOperator.Equal)
            {
                isConditionMet = variable.Value == condition.RightOperandValue;
            }
            else
            {
                isConditionMet = variable.Value != condition.RightOperandValue;
            }

            return isConditionMet;
        }
    }
}
