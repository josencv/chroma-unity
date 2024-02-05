using System;
using System.Collections.Generic;
using Chroma.Core.Infrastructure.StateMachines;
using NUnit.Framework;

namespace Chroma.Tests.Unit.Core.Infrastructure.StateMachines
{
    public class StateMachineTests
    {
        private StateMachine stateMachine;
        const string movementSpeedVarName = "movementSpeed";
        private State idleState;
        private State walkingState;
        private State runningState;

        [SetUp]
        public void SetUp()
        {
            this.idleState = new EmptyState("Idle");
            this.walkingState = new EmptyState("Walking");
            this.runningState = new EmptyState("Running");

            var idleToWalkingTransition = new StateTransition(
                this.walkingState,
                new List<TransitionCondition>
                {
                    TransitionCondition.CreateFloatCondition(movementSpeedVarName, ConditionOperator.Greater, 0)
                }
            );

            var walkingToRunningTransition = new StateTransition(
                this.runningState,
                new List<TransitionCondition>
                {
                    TransitionCondition.CreateFloatCondition(movementSpeedVarName, ConditionOperator.Greater, 1.0f)
                }
            );

            var runningToWalkingTransition = new StateTransition(
                this.walkingState,
                new List<TransitionCondition>
                {
                    TransitionCondition.CreateFloatCondition(movementSpeedVarName, ConditionOperator.LessEqual, 1.0f)
                }
            );

            var walkingToIdleTransition = new StateTransition(
                this.idleState,
                new List<TransitionCondition>
                {
                    TransitionCondition.CreateFloatCondition(movementSpeedVarName, ConditionOperator.Equal, 0)
                }
            );

            this.idleState.AddTransition(idleToWalkingTransition);
            this.walkingState.AddTransition(walkingToRunningTransition);
            this.runningState.AddTransition(runningToWalkingTransition);
            this.walkingState.AddTransition(walkingToIdleTransition);

            this.stateMachine = new StateMachine(this.idleState);
            this.stateMachine.RegisterFloat(movementSpeedVarName, 0.0f);
        }

        [Test]
        [Category("Start")]
        public void Start_SetsCurrentStateToTheEntrypoint()
        {
            this.stateMachine.Start();

            Assert.AreEqual(this.idleState, this.stateMachine.CurrentState);
        }

        [Test]
        [Category("Tick")]
        public void Tick_ChangesStatesCorrectly()
        {
            this.stateMachine.Start();
            Assert.AreEqual(this.stateMachine.CurrentState, this.idleState);

            this.stateMachine.Tick(0.01f);
            Assert.AreEqual(this.stateMachine.CurrentState, this.idleState);

            this.stateMachine.UpdateFloat(movementSpeedVarName, 0.5f);
            this.stateMachine.Tick(0.01f);
            Assert.AreEqual(this.stateMachine.CurrentState, this.walkingState);

            this.stateMachine.UpdateFloat(movementSpeedVarName, 1.01f);
            this.stateMachine.Tick(0.01f);
            Assert.AreEqual(this.stateMachine.CurrentState, this.runningState);

            this.stateMachine.UpdateFloat(movementSpeedVarName, 1.0f);
            this.stateMachine.Tick(0.01f);
            Assert.AreEqual(this.stateMachine.CurrentState, this.walkingState);

            this.stateMachine.UpdateFloat(movementSpeedVarName, 0);
            this.stateMachine.Tick(0.01f);
            Assert.AreEqual(this.stateMachine.CurrentState, this.idleState);
        }

        [Test]
        [Category("Tick")]
        public void Tick_ChangesMultipleStatesInOneTick()
        {
            this.stateMachine.Start();
            this.stateMachine.Tick(0.01f);

            this.stateMachine.UpdateFloat(movementSpeedVarName, 1.01f);
            this.stateMachine.Tick(0.01f);
            Assert.AreEqual(this.stateMachine.CurrentState, this.runningState);

            this.stateMachine.UpdateFloat(movementSpeedVarName, 0);
            this.stateMachine.Tick(0.01f);
            Assert.AreEqual(this.stateMachine.CurrentState, this.idleState);
        }

        [Test]
        [Category("Tick")]
        public void Tick_ChangesStatesCorrectlyForBoolConditions()
        {
            string isGroundedVarName = "isGrounded";
            var jumpingState = new EmptyState("Jumping");
            var idleToJumping = new StateTransition(jumpingState, new List<TransitionCondition> {
                TransitionCondition.CreateBoolCondition(isGroundedVarName, ConditionOperator.NotEqual, true)
            });
            var jumpingToIdle = new StateTransition(this.idleState, new List<TransitionCondition> {
                TransitionCondition.CreateBoolCondition(isGroundedVarName, ConditionOperator.Equal, true)
            });
            this.idleState.AddTransition(idleToJumping);
            jumpingState.AddTransition(jumpingToIdle);
            this.stateMachine.RegisterBool(isGroundedVarName, true);
            this.stateMachine.Start();

            this.stateMachine.UpdateBool(isGroundedVarName, false);
            this.stateMachine.Tick(0.01f);
            Assert.AreEqual(this.stateMachine.CurrentState, jumpingState);

            this.stateMachine.UpdateBool(isGroundedVarName, true);
            this.stateMachine.Tick(0.01f);
            Assert.AreEqual(this.stateMachine.CurrentState, this.idleState);
        }

        [Test]
        [Category("Tick")]
        public void Tick_ChangesStatesCorrectlyForIntConditions()
        {
            string chargeLevelVarName = "chargeLevel";
            var chargingState = new EmptyState("Charging");
            var idleToCharging = new StateTransition(chargingState, new List<TransitionCondition> {
                TransitionCondition.CreateIntCondition(chargeLevelVarName, ConditionOperator.Greater, 0)
            });
            var charginToIdle = new StateTransition(this.idleState, new List<TransitionCondition> {
                TransitionCondition.CreateIntCondition(chargeLevelVarName, ConditionOperator.Equal, 0)
            });
            this.idleState.AddTransition(idleToCharging);
            chargingState.AddTransition(charginToIdle);
            this.stateMachine.RegisterInt(chargeLevelVarName, 0);
            this.stateMachine.Start();

            this.stateMachine.UpdateInt(chargeLevelVarName, 1);
            this.stateMachine.Tick(0.01f);
            Assert.AreEqual(this.stateMachine.CurrentState, chargingState);

            this.stateMachine.UpdateInt(chargeLevelVarName, 0);
            this.stateMachine.Tick(0.01f);
            Assert.AreEqual(this.stateMachine.CurrentState, this.idleState);
        }

        [Test]
        [Category("Tick")]
        public void Tick_ChangesStatesCorrectlyForTriggerConditions()
        {
            string attackVarName = "attack";
            var attackingState = new EmptyState("Attacking");
            var comboingState = new EmptyState("Comboing");
            var idleToAtacking = new StateTransition(attackingState, new List<TransitionCondition> {
                TransitionCondition.CreateTriggerCondition(attackVarName)
            });

            // Added this transition to test that a trigger does not span 3 stats.
            // Maybe this should go in it's own test
            var attackingToComboing = new StateTransition(comboingState, new List<TransitionCondition> {
                TransitionCondition.CreateTriggerCondition(attackVarName)
            });

            // TODO: add "on state/action end" event as a condition for transitions, so the "attack" can
            // return to a previous state "naturally"

            this.idleState.AddTransition(idleToAtacking);
            attackingState.AddTransition(attackingToComboing);
            this.stateMachine.RegisterTrigger(attackVarName);
            this.stateMachine.Start();

            this.stateMachine.ActivateTrigger(attackVarName);
            this.stateMachine.Tick(0.01f);
            Assert.AreEqual(this.stateMachine.CurrentState, attackingState);
        }

        [Test]
        [Category("Tick")]
        public void Tick_DetectsLoopsInEvaluatedStates()
        {
            var walkingToIdle = new StateTransition(this.idleState, new List<TransitionCondition> {
                TransitionCondition.CreateFloatCondition(movementSpeedVarName, ConditionOperator.Less, 0.3f)
            });

            this.walkingState.AddTransition(walkingToIdle);
            this.stateMachine.Start();

            this.stateMachine.UpdateFloat(movementSpeedVarName, 0.2f);

            ApplicationException exception = Assert.Throws<ApplicationException>(() => this.stateMachine.Tick(0.01f));
        }
    }
}
