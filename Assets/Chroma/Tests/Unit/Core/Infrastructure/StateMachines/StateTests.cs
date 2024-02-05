using System;
using System.Collections.Generic;
using System.Linq;
using Chroma.Core.Infrastructure.StateMachines;
using NUnit.Framework;

namespace Chroma.Tests.Unit.Core.Infrastructure.StateMachines
{
    public class StateTests
    {
        [Test]
        [Category("Name")]
        public void Name_GetsValueCorrectly()
        {
            string stateName = "Test State";
            var state = new EmptyState(stateName);

            Assert.AreEqual(stateName, state.Name);
        }

        [Test]
        [Category("Transitions")]
        public void Transitions_GetsValueCorrectly()
        {
            var firstState = new EmptyState("First");

            Assert.IsEmpty(firstState.Transitions);
        }

        [Test]
        [Category("AddTransition")]
        public void AddTransition_AddsTransitionCorrectly()
        {
            var firstState = new EmptyState("First");
            var secondState = new EmptyState("Second");
            var transition = new StateTransition(secondState, new List<TransitionCondition>());

            firstState.AddTransition(transition);

            Assert.AreEqual(transition, firstState.Transitions.First());
        }

        [Test]
        [Category("AddTransition")]
        public void AddTransition_ThrowsIfTheTransitionPointsToItself()
        {
            var firstState = new EmptyState("First");
            var transition = new StateTransition(firstState, new List<TransitionCondition>());

            ArgumentException exception = Assert.Throws<ArgumentException>(() => firstState.AddTransition(transition));

            Assert.That(exception.Message, Does.Contain("points to itself"));
        }

        [Test]
        [Category("EvaluateTransitions")]
        public void EvaluateTransitions_ReturnsTransitionIfConditionsAreMet()
        {
            string isGroundedVarName = "isGrounded";
            var blackboard = new Blackboard();
            var firstState = new EmptyState("First");
            var secondState = new EmptyState("Second");
            var transition = new StateTransition(secondState, new List<TransitionCondition> {
                TransitionCondition.CreateBoolCondition(isGroundedVarName, ConditionOperator.NotEqual, true),
            });

            blackboard.RegisterBool(isGroundedVarName, false);
            firstState.AddTransition(transition);

            StateTransition foundTransition = firstState.EvaluateTransitions(blackboard);

            Assert.AreEqual(transition, foundTransition);
        }

        [Test]
        [Category("EvaluateTransitions")]
        public void EvaluateTransitions_ReturnsNullIfConditionsAreNotMet()
        {
            string isGroundedVarName = "isGrounded";
            var blackboard = new Blackboard();
            var firstState = new EmptyState("First");
            var secondState = new EmptyState("Second");
            var transition = new StateTransition(secondState, new List<TransitionCondition> {
                TransitionCondition.CreateBoolCondition(isGroundedVarName, ConditionOperator.NotEqual, false),
            });

            blackboard.RegisterBool(isGroundedVarName, false);
            firstState.AddTransition(transition);

            StateTransition foundTransition = firstState.EvaluateTransitions(blackboard);

            Assert.IsNull(foundTransition);
        }

        [Test]
        [Category("EvaluateTransitions")]
        public void EvaluateTransitions_ReturnsNullIfThereAreNoTransitions()
        {
            var blackboard = new Blackboard();
            var firstState = new EmptyState("First");

            StateTransition foundTransition = firstState.EvaluateTransitions(blackboard);

            Assert.IsNull(foundTransition);
        }
    }
}
