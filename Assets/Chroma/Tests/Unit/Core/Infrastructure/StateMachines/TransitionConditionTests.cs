using System;
using Chroma.Core.Infrastructure.StateMachines;
using NUnit.Framework;

namespace Chroma.Tests.Unit.Core.Infrastructure.StateMachines
{
    public class TransitionConditionTests
    {
        [Test]
        [Category("CreateBoolCondition")]
        public void CreateBoolCondition_CreatesConditionCorrectlyWithTrueValue()
        {
            string varName = "prop";
            ConditionOperator conditionOperator = ConditionOperator.Equal;
            bool value = true;

            TransitionCondition condition = TransitionCondition.CreateBoolCondition(varName, conditionOperator, value);

            Assert.AreEqual(varName, condition.LeftOperandVarName);
            Assert.AreEqual(conditionOperator, condition.ConditionOperator);
            Assert.AreEqual(1.0f, condition.RightOperandValue);
        }

        [Test]
        [Category("CreateBoolCondition")]
        public void CreateBoolCondition_CreatesConditionCorrectlyWithFalseValue()
        {
            string varName = "prop";
            ConditionOperator conditionOperator = ConditionOperator.Equal;
            bool value = false;

            TransitionCondition condition = TransitionCondition.CreateBoolCondition(varName, conditionOperator, value);

            Assert.AreEqual(varName, condition.LeftOperandVarName);
            Assert.AreEqual(conditionOperator, condition.ConditionOperator);
            Assert.AreEqual(0.0f, condition.RightOperandValue);
        }

        [Test]
        [Category("CreateBoolCondition")]
        public void CreateBoolCondition_ThrowsIfOperatorIsNotValid()
        {
            Assert.Throws<ArgumentException>(() => TransitionCondition.CreateBoolCondition("prop", ConditionOperator.Greater, true));
            Assert.Throws<ArgumentException>(() => TransitionCondition.CreateBoolCondition("prop", ConditionOperator.Less, true));
        }

        [Test]
        [Category("CreateFloatCondition")]
        public void CreateFloatCondition_CreatesCondition()
        {
            string varName = "prop";
            ConditionOperator conditionOperator = ConditionOperator.Equal;
            float value = 1.8f;

            TransitionCondition condition = TransitionCondition.CreateFloatCondition(varName, conditionOperator, value);

            Assert.AreEqual(varName, condition.LeftOperandVarName);
            Assert.AreEqual(conditionOperator, condition.ConditionOperator);
            Assert.AreEqual(value, condition.RightOperandValue);
        }

        [Test]
        [Category("CreateIntCondition")]
        public void CreateIntCondition_CreatesCondition()
        {
            string varName = "prop";
            ConditionOperator conditionOperator = ConditionOperator.Equal;
            int value = 2;

            TransitionCondition condition = TransitionCondition.CreateIntCondition(varName, conditionOperator, value);

            Assert.AreEqual(varName, condition.LeftOperandVarName);
            Assert.AreEqual(conditionOperator, condition.ConditionOperator);
            Assert.AreEqual(value, condition.RightOperandValue);
        }

        [Test]
        [Category("CreateTriggerCondition")]
        public void CreateTriggerCondition_CreatesCondition()
        {
            string varName = "prop";

            TransitionCondition condition = TransitionCondition.CreateTriggerCondition(varName);

            Assert.AreEqual(varName, condition.LeftOperandVarName);
            Assert.AreEqual(ConditionOperator.Equal, condition.ConditionOperator);
            Assert.AreEqual(1.0f, condition.RightOperandValue);
        }
    }
}
