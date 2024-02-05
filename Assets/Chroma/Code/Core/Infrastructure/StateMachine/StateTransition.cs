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

        private bool IsConditionMet(Blackboard blackboard, TransitionCondition condition)
        {
            BlackboardVariable variable = blackboard.GetVariable(condition.LeftOperandVarName);
            bool isConditionMet;

            switch(condition.ConditionOperator)
            {
                case ConditionOperator.Greater:
                    isConditionMet = variable.Value > condition.RightOperandValue;
                    break;
                case ConditionOperator.GreaterEqual:
                    isConditionMet = variable.Value >= condition.RightOperandValue;
                    break;
                case ConditionOperator.Less:
                    isConditionMet = variable.Value < condition.RightOperandValue;
                    break;
                case ConditionOperator.LessEqual:
                    isConditionMet = variable.Value <= condition.RightOperandValue;
                    break;
                case ConditionOperator.Equal:
                    isConditionMet = variable.Value == condition.RightOperandValue;
                    break;
                default:
                    isConditionMet = variable.Value != condition.RightOperandValue;
                    break;
            }

            return isConditionMet;
        }
    }
}
