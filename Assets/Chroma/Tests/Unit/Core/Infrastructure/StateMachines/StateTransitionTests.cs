using System.Collections.Generic;
using Chroma.Core.Infrastructure.StateMachines;
using NUnit.Framework;

namespace Chroma.Tests.Unit.Core.Infrastructure.StateMachines
{
    public class StateTransitionTests
    {
        private Blackboard blackboard;
        private List<TransitionCondition> conditions;
        const string isGroundedVarName = "isGrounded";
        const string movementSpeedVarName = "movementSpeed";
        const string chargeLevelVarName = "chargeLevel";
        const bool isGroundedInitialValue = true;
        const float movementSpeedInitialValue = 1.0f;
        const int chargeLevelInitialValue = 3;

        [SetUp]
        public void SetUp()
        {
            this.blackboard = new Blackboard();
            this.blackboard.RegisterBool(isGroundedVarName, isGroundedInitialValue);
            this.blackboard.RegisterFloat(movementSpeedVarName, movementSpeedInitialValue);
            this.blackboard.RegisterInt(chargeLevelVarName, chargeLevelInitialValue);
            this.conditions = new List<TransitionCondition>
            {
                TransitionCondition.CreateBoolCondition(isGroundedVarName, ConditionOperator.NotEqual, isGroundedInitialValue),
                TransitionCondition.CreateFloatCondition(movementSpeedVarName, ConditionOperator.Greater, movementSpeedInitialValue),
                TransitionCondition.CreateIntCondition(chargeLevelVarName, ConditionOperator.Greater, chargeLevelInitialValue),
            };
        }

        [Test]
        [Category("AreTransitionConditionsMet")]
        public void AreTransitionConditionsMet_ReturnsTrueIfAllConditionsAreMet()
        {
            StateTransition transition = new StateTransition(new EmptyState("Empty"), this.conditions);
            this.blackboard.UpdateBool(isGroundedVarName, !isGroundedInitialValue);
            this.blackboard.UpdateFloat(movementSpeedVarName, movementSpeedInitialValue + 1.0f);
            this.blackboard.UpdateInt(chargeLevelVarName, chargeLevelInitialValue + 1);

            bool conditionsMet = transition.AreTransitionConditionsMet(this.blackboard);

            Assert.AreEqual(true, conditionsMet);
        }

        [Test]
        [Category("AreTransitionConditionsMet")]
        public void AreTransitionConditionsMet_ReturnsFalseIfNotAllConditionsAreMet()
        {
            StateTransition transition = new StateTransition(new EmptyState("Empty"), this.conditions);
            this.blackboard.UpdateBool(isGroundedVarName, !isGroundedInitialValue);

            bool conditionsMet = transition.AreTransitionConditionsMet(this.blackboard);

            Assert.AreEqual(false, conditionsMet);
        }

        [Test]
        [Category("AreTransitionConditionsMet")]
        public void AreTransitionConditionsMet_ReturnsFalseIfNoConditionIsMet()
        {
            StateTransition transition = new StateTransition(new EmptyState("Empty"), this.conditions);
            this.blackboard.UpdateBool(isGroundedVarName, !isGroundedInitialValue);

            bool conditionsMet = transition.AreTransitionConditionsMet(this.blackboard);

            Assert.AreEqual(false, conditionsMet);
        }

        [Test]
        [Category("AreTransitionConditionsMet")]
        public void AreTransitionConditionsMet_GreaterOperatorWorksCorrectly()
        {
            var conditions = new List<TransitionCondition>
            {
                TransitionCondition.CreateFloatCondition(movementSpeedVarName, ConditionOperator.Greater, movementSpeedInitialValue),
            };

            StateTransition transition = new StateTransition(new EmptyState("Empty"), conditions);


            bool equal = transition.AreTransitionConditionsMet(this.blackboard);
            this.blackboard.UpdateFloat(movementSpeedVarName, movementSpeedInitialValue + 0.1f);
            bool greater = transition.AreTransitionConditionsMet(this.blackboard);
            this.blackboard.UpdateFloat(movementSpeedVarName, movementSpeedInitialValue - 0.1f);
            bool less = transition.AreTransitionConditionsMet(this.blackboard);

            Assert.AreEqual(false, equal);
            Assert.AreEqual(true, greater);
            Assert.AreEqual(false, less);
        }

        [Test]
        [Category("AreTransitionConditionsMet")]
        public void AreTransitionConditionsMet_GreaterEqualOperatorWorksCorrectly()
        {
            var conditions = new List<TransitionCondition>
            {
                TransitionCondition.CreateFloatCondition(movementSpeedVarName, ConditionOperator.GreaterEqual, movementSpeedInitialValue),
            };

            StateTransition transition = new StateTransition(new EmptyState("Empty"), conditions);


            bool equal = transition.AreTransitionConditionsMet(this.blackboard);
            this.blackboard.UpdateFloat(movementSpeedVarName, movementSpeedInitialValue + 0.1f);
            bool greater = transition.AreTransitionConditionsMet(this.blackboard);
            this.blackboard.UpdateFloat(movementSpeedVarName, movementSpeedInitialValue - 0.1f);
            bool less = transition.AreTransitionConditionsMet(this.blackboard);

            Assert.AreEqual(true, equal);
            Assert.AreEqual(true, greater);
            Assert.AreEqual(false, less);
        }

        [Test]
        [Category("AreTransitionConditionsMet")]
        public void AreTransitionConditionsMet_LessOperatorWorksCorrectly()
        {
            var conditions = new List<TransitionCondition>
            {
                TransitionCondition.CreateFloatCondition(movementSpeedVarName, ConditionOperator.Less, movementSpeedInitialValue),
            };

            StateTransition transition = new StateTransition(new EmptyState("Empty"), conditions);
            bool equal = transition.AreTransitionConditionsMet(this.blackboard);
            this.blackboard.UpdateFloat(movementSpeedVarName, movementSpeedInitialValue - 0.1f);
            bool less = transition.AreTransitionConditionsMet(this.blackboard);
            this.blackboard.UpdateFloat(movementSpeedVarName, movementSpeedInitialValue + 0.1f);
            bool greater = transition.AreTransitionConditionsMet(this.blackboard);

            Assert.AreEqual(false, equal);
            Assert.AreEqual(true, less);
            Assert.AreEqual(false, greater);
        }

        [Test]
        [Category("AreTransitionConditionsMet")]
        public void AreTransitionConditionsMet_LessEqualOperatorWorksCorrectly()
        {
            var conditions = new List<TransitionCondition>
            {
                TransitionCondition.CreateFloatCondition(movementSpeedVarName, ConditionOperator.LessEqual, movementSpeedInitialValue),
            };

            StateTransition transition = new StateTransition(new EmptyState("Empty"), conditions);
            bool equal = transition.AreTransitionConditionsMet(this.blackboard);
            this.blackboard.UpdateFloat(movementSpeedVarName, movementSpeedInitialValue - 0.1f);
            bool less = transition.AreTransitionConditionsMet(this.blackboard);
            this.blackboard.UpdateFloat(movementSpeedVarName, movementSpeedInitialValue + 0.1f);
            bool greater = transition.AreTransitionConditionsMet(this.blackboard);

            Assert.AreEqual(true, equal);
            Assert.AreEqual(true, less);
            Assert.AreEqual(false, greater);
        }

        [Test]
        [Category("AreTransitionConditionsMet")]
        public void AreTransitionConditionsMet_EqualOperatorWorksCorrectly()
        {
            var conditions = new List<TransitionCondition>
            {
                TransitionCondition.CreateFloatCondition(movementSpeedVarName, ConditionOperator.Equal, movementSpeedInitialValue),
            };

            StateTransition transition = new StateTransition(new EmptyState("Empty"), conditions);
            bool equal = transition.AreTransitionConditionsMet(this.blackboard);
            this.blackboard.UpdateFloat(movementSpeedVarName, movementSpeedInitialValue + 0.1f);
            bool greater = transition.AreTransitionConditionsMet(this.blackboard);
            this.blackboard.UpdateFloat(movementSpeedVarName, movementSpeedInitialValue - 0.1f);
            bool less = transition.AreTransitionConditionsMet(this.blackboard);

            Assert.AreEqual(true, equal);
            Assert.AreEqual(false, greater);
            Assert.AreEqual(false, less);
        }

        [Test]
        [Category("AreTransitionConditionsMet")]
        public void AreTransitionConditionsMet_NotEqualOperatorWorksCorrectly()
        {
            var conditions = new List<TransitionCondition>
            {
                TransitionCondition.CreateFloatCondition(movementSpeedVarName, ConditionOperator.NotEqual, movementSpeedInitialValue),
            };

            StateTransition transition = new StateTransition(new EmptyState("Empty"), conditions);
            bool equal = transition.AreTransitionConditionsMet(this.blackboard);
            this.blackboard.UpdateFloat(movementSpeedVarName, movementSpeedInitialValue + 0.1f);
            bool greater = transition.AreTransitionConditionsMet(this.blackboard);
            this.blackboard.UpdateFloat(movementSpeedVarName, movementSpeedInitialValue - 0.1f);
            bool less = transition.AreTransitionConditionsMet(this.blackboard);

            Assert.AreEqual(false, equal);
            Assert.AreEqual(true, greater);
            Assert.AreEqual(true, less);
        }
    }
}
