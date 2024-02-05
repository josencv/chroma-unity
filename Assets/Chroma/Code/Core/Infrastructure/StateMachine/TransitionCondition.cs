using System;

namespace Chroma.Core.Infrastructure.StateMachines
{
    public readonly struct TransitionCondition
    {
        public string LeftOperandVarName { get; }
        public ConditionOperator ConditionOperator { get; }
        public float RightOperandValue { get; }

        private TransitionCondition(string leftOperandVarName, ConditionOperator conditionOperator, float rightOperandValue)
        {
            this.LeftOperandVarName = leftOperandVarName;
            this.ConditionOperator = conditionOperator;
            this.RightOperandValue = rightOperandValue;
        }

        public static TransitionCondition CreateBoolCondition(string leftOperandVarName, ConditionOperator conditionOperator, bool rightOperandValue)
        {
            if(conditionOperator != ConditionOperator.Equal && conditionOperator != ConditionOperator.NotEqual)
            {
                throw new ArgumentException($"the condition operator is not valid for bool transition condition.");
            }

            float value = rightOperandValue == true ? 1.0f : 0.0f;
            return new TransitionCondition(leftOperandVarName, conditionOperator, value);
        }

        public static TransitionCondition CreateFloatCondition(string leftOperandVarName, ConditionOperator conditionOperator, float rightOperandValue)
        {
            return new TransitionCondition(leftOperandVarName, conditionOperator, rightOperandValue);
        }

        public static TransitionCondition CreateIntCondition(string leftOperandVarName, ConditionOperator conditionOperator, int rightOperandValue)
        {
            return new TransitionCondition(leftOperandVarName, conditionOperator, rightOperandValue);
        }

        public static TransitionCondition CreateTriggerCondition(string leftOperandVarName)
        {
            return new TransitionCondition(leftOperandVarName, ConditionOperator.Equal, 1.0f);
        }
    }
}
