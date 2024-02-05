using System;
using Chroma.Core.Infrastructure.StateMachines;
using NUnit.Framework;

namespace Chroma.Tests.Unit.Core.Infrastructure.StateMachines
{
    public class BlackboardTests
    {
        [Test]
        [Category("GetVariable")]
        public void GetVariable_ReturnsVariableCorrectly()
        {
            string varName = "prop";
            float value = 1.1f;
            var blackboard = new Blackboard();

            blackboard.RegisterFloat(varName, value);
            BlackboardVariable returnedVar = blackboard.GetVariable(varName);

            Assert.AreEqual(returnedVar.Type, BlackboardVariableType.Float);
            Assert.AreEqual(value, returnedVar.Value);
        }

        [Test]
        [Category("GetVariable")]
        public void GetVariable_ThrowsIfVariableDoesNotExist()
        {
            string varName = "prop";
            var blackboard = new Blackboard();

            ApplicationException exception = Assert.Throws<ApplicationException>(() => blackboard.GetVariable(varName));

            Assert.That(exception.Message, Does.Contain("does not exist"));
        }

        [Test]
        [Category("RegisterBool")]
        public void RegisterBool_RegistersVariableCorrectly()
        {
            string varName = "boolProp";
            bool varValue = true;
            var blackboard = new Blackboard();

            blackboard.RegisterBool(varName, varValue);
            BlackboardVariable returnedVar = blackboard.GetVariable(varName);

            Assert.AreEqual(returnedVar.Type, BlackboardVariableType.Bool);
            Assert.AreEqual(1.0f, returnedVar.Value);
        }

        [Test]
        [Category("RegisterBool")]
        public void RegisterBool_RegistersVariableWithDefaultValueOfZero()
        {
            string varName = "boolProp";
            var blackboard = new Blackboard();

            blackboard.RegisterBool(varName);
            BlackboardVariable returnedVar = blackboard.GetVariable(varName);

            Assert.AreEqual(returnedVar.Type, BlackboardVariableType.Bool);
            Assert.AreEqual(0.0f, returnedVar.Value);
        }

        [Test]
        [Category("RegisterBool")]
        public void RegisterBool_ThrowsIfTheVariableAlreadyExists()
        {
            string varName = "boolProp";
            bool varValue = true;
            var blackboard = new Blackboard();

            blackboard.RegisterBool(varName, varValue);

            ApplicationException exception = Assert.Throws<ApplicationException>(() => blackboard.RegisterBool(varName, varValue));

            Assert.That(exception.Message, Does.Contain("already exists"));
        }

        [Test]
        [Category("RegisterFloat")]
        public void RegisterFloat_RegistersVariableCorrectly()
        {
            string varName = "floatProp";
            float varValue = 1.1f;
            var blackboard = new Blackboard();

            blackboard.RegisterFloat(varName, varValue);
            BlackboardVariable returnedVar = blackboard.GetVariable(varName);

            Assert.AreEqual(returnedVar.Type, BlackboardVariableType.Float);
            Assert.AreEqual(varValue, returnedVar.Value);
        }

        [Test]
        [Category("RegisterFloat")]
        public void RegisterFloat_RegistersVariableWithDefaultValueOfZero()
        {
            string varName = "floatProp";
            var blackboard = new Blackboard();

            blackboard.RegisterFloat(varName);
            BlackboardVariable returnedVar = blackboard.GetVariable(varName);

            Assert.AreEqual(returnedVar.Type, BlackboardVariableType.Float);
            Assert.AreEqual(0.0f, returnedVar.Value);
        }

        [Test]
        [Category("RegisterFloat")]
        public void RegisterFloat_ThrowsIfTheVariableAlreadyExists()
        {
            string varName = "floatProp";
            float varValue = 1.1f;
            var blackboard = new Blackboard();

            blackboard.RegisterFloat(varName, varValue);

            ApplicationException exception = Assert.Throws<ApplicationException>(() => blackboard.RegisterFloat(varName, varValue));

            Assert.That(exception.Message, Does.Contain("already exists"));
        }

        [Test]
        [Category("RegisterInt")]
        public void RegisterInt_RegistersVariableCorrectly()
        {
            string varName = "intProp";
            int varValue = 2;
            var blackboard = new Blackboard();

            blackboard.RegisterInt(varName, varValue);
            BlackboardVariable returnedVar = blackboard.GetVariable(varName);

            Assert.AreEqual(returnedVar.Type, BlackboardVariableType.Int);
            Assert.AreEqual(varValue, returnedVar.Value);
        }

        [Test]
        [Category("RegisterInt")]
        public void RegisterInt_RegistersVariableWithDefaultValueOfZero()
        {
            string varName = "intProp";
            var blackboard = new Blackboard();

            blackboard.RegisterInt(varName);
            BlackboardVariable returnedVar = blackboard.GetVariable(varName);

            Assert.AreEqual(returnedVar.Type, BlackboardVariableType.Int);
            Assert.AreEqual(0.0f, returnedVar.Value);
        }

        [Test]
        [Category("RegisterInt")]
        public void RegisterInt_ThrowsIfTheVariableAlreadyExists()
        {
            string varName = "intProp";
            int varValue = 2;
            var blackboard = new Blackboard();

            blackboard.RegisterInt(varName, varValue);

            ApplicationException exception = Assert.Throws<ApplicationException>(() => blackboard.RegisterInt(varName, varValue));

            Assert.That(exception.Message, Does.Contain("already exists"));
        }

        [Test]
        [Category("RegisterTrigger")]
        public void RegisterTrigger_RegistersVariableCorrectly()
        {
            string varName = "triggerProp";
            var blackboard = new Blackboard();

            blackboard.RegisterTrigger(varName);
            BlackboardVariable returnedVar = blackboard.GetVariable(varName);

            Assert.AreEqual(returnedVar.Type, BlackboardVariableType.Trigger);
            Assert.AreEqual(0.0f, returnedVar.Value);
        }

        [Test]
        [Category("RegisterTrigger")]
        public void RegisterTrigger_ThrowsIfTheVariableAlreadyExists()
        {
            string varName = "triggerProp";
            var blackboard = new Blackboard();

            blackboard.RegisterTrigger(varName);

            ApplicationException exception = Assert.Throws<ApplicationException>(() => blackboard.RegisterTrigger(varName));

            Assert.That(exception.Message, Does.Contain("already exists"));
        }

        [Test]
        [Category("UpdateBool")]
        public void UpdateBool_UpdatesVariableCorrectly()
        {
            string varName = "boolProp";
            bool varValue = true;
            var blackboard = new Blackboard();

            blackboard.RegisterBool(varName, !varValue);
            blackboard.UpdateBool(varName, varValue);
            BlackboardVariable returnedVar = blackboard.GetVariable(varName);

            Assert.AreEqual(returnedVar.Type, BlackboardVariableType.Bool);
            Assert.AreEqual(1.0f, returnedVar.Value);
        }

        [Test]
        [Category("UpdateBool")]
        public void UpdateBool_ThrowsIfTheVariableDoesNotExist()
        {
            string varName = "boolProp";
            var blackboard = new Blackboard();

            ApplicationException exception = Assert.Throws<ApplicationException>(() => blackboard.UpdateBool(varName, true));

            Assert.That(exception.Message, Does.Contain("does not exist"));
        }

        [Test]
        [Category("UpdateBool")]
        public void UpdateBool_ThrowsIfVariableTypeIsIncorrect()
        {
            string varName = "floatProp";
            var blackboard = new Blackboard();
            blackboard.RegisterFloat(varName, 1);

            ApplicationException exception = Assert.Throws<ApplicationException>(() => blackboard.UpdateBool(varName, true));

            Assert.That(exception.Message, Does.Contain("incorrect type"));
        }

        [Test]
        [Category("UpdateFloat")]
        public void UpdateFloat_UpdatesVariableCorrectly()
        {
            string varName = "floatProp";
            float varValue = 1.1f;
            var blackboard = new Blackboard();

            blackboard.RegisterFloat(varName, varValue + 1.0f);
            blackboard.UpdateFloat(varName, varValue);
            BlackboardVariable returnedVar = blackboard.GetVariable(varName);

            Assert.AreEqual(returnedVar.Type, BlackboardVariableType.Float);
            Assert.AreEqual(varValue, returnedVar.Value);
        }

        [Test]
        [Category("UpdateFloat")]
        public void UpdateFloat_ThrowsIfTheVariableDoesNotExist()
        {
            string varName = "floatProp";
            var blackboard = new Blackboard();

            ApplicationException exception = Assert.Throws<ApplicationException>(() => blackboard.UpdateFloat(varName, 1.1f));

            Assert.That(exception.Message, Does.Contain("does not exist"));
        }

        [Test]
        [Category("UpdateFloat")]
        public void UpdateFloat_ThrowsIfVariableTypeIsIncorrect()
        {
            string varName = "floatProp";
            var blackboard = new Blackboard();
            blackboard.RegisterBool(varName, true);

            ApplicationException exception = Assert.Throws<ApplicationException>(() => blackboard.UpdateFloat(varName, 1.0f));

            Assert.That(exception.Message, Does.Contain("incorrect type"));
        }

        [Test]
        [Category("UpdateInt")]
        public void UpdateInt_UpdatesVariableCorrectly()
        {
            string varName = "intProp";
            int varValue = 2;
            var blackboard = new Blackboard();

            blackboard.RegisterInt(varName, varValue + 1);
            blackboard.UpdateInt(varName, varValue);
            BlackboardVariable returnedVar = blackboard.GetVariable(varName);

            Assert.AreEqual(returnedVar.Type, BlackboardVariableType.Int);
            Assert.AreEqual(varValue, returnedVar.Value);
        }

        [Test]
        [Category("UpdateInt")]
        public void UpdateInt_ThrowsIfTheVariableDoesNotExist()
        {
            string varName = "intProp";
            var blackboard = new Blackboard();

            ApplicationException exception = Assert.Throws<ApplicationException>(() => blackboard.UpdateInt(varName, 1));

            Assert.That(exception.Message, Does.Contain("does not exist"));
        }

        [Test]
        [Category("UpdateInt")]
        public void UpdateInt_ThrowsIfVariableTypeIsIncorrect()
        {
            string varName = "floatProp";
            var blackboard = new Blackboard();
            blackboard.RegisterBool(varName, true);

            ApplicationException exception = Assert.Throws<ApplicationException>(() => blackboard.UpdateInt(varName, 1));

            Assert.That(exception.Message, Does.Contain("incorrect type"));
        }

        [Test]
        [Category("ActivateTrigger")]
        public void ActivateTrigger_UpdatesVariableCorrectly()
        {
            string varName = "triggerProp";
            var blackboard = new Blackboard();

            blackboard.RegisterTrigger(varName);
            blackboard.ActivateTrigger(varName);
            BlackboardVariable returnedVar = blackboard.GetVariable(varName);

            Assert.AreEqual(returnedVar.Type, BlackboardVariableType.Trigger);
            Assert.AreEqual(1.0f, returnedVar.Value);
        }

        [Test]
        [Category("ActivateTrigger")]
        public void ActivateTrigger_ThrowsIfTheVariableDoesNotExist()
        {
            string varName = "triggerProp";
            var blackboard = new Blackboard();

            ApplicationException exception = Assert.Throws<ApplicationException>(() => blackboard.ActivateTrigger(varName));

            Assert.That(exception.Message, Does.Contain("does not exist"));
        }

        [Test]
        [Category("ActivateTrigger")]
        public void ActivateTrigger_ThrowsIfVariableTypeIsIncorrect()
        {
            string varName = "floatProp";
            var blackboard = new Blackboard();
            blackboard.RegisterBool(varName, true);

            ApplicationException exception = Assert.Throws<ApplicationException>(() => blackboard.ActivateTrigger(varName));

            Assert.That(exception.Message, Does.Contain("incorrect type"));
        }

        [Test]
        [Category("CleanTriggers")]
        public void CleanTriggers_ThrowsIfTheVariableDoesNotExist()
        {
            string varName = "triggerProp";
            var blackboard = new Blackboard();
            blackboard.RegisterTrigger(varName);
            blackboard.ActivateTrigger(varName);
            blackboard.CleanTriggers();

            BlackboardVariable returnedVar = blackboard.GetVariable(varName);

            Assert.AreEqual(0.0f, returnedVar.Value);
        }
    }
}
