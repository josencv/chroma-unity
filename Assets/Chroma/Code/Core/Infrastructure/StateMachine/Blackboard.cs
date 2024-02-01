using System;
using System.Collections.Generic;

namespace Chroma.Core.Infrastructure.StateMachines
{
    public class Blackboard
    {
        public Dictionary<string, BlackboardVariable> Variables { get; } = new Dictionary<string, BlackboardVariable>();
        private List<string> activeTriggerNames = new List<string>();

        /// <summary>
        /// Sets the triggers to false.
        /// Note that it does not remove the entries.
        /// </summary>
        public void CleanTriggers()
        {
            foreach(string triggerName in this.activeTriggerNames)
            {
                this.Variables[triggerName].Value = 0;
            }

            this.activeTriggerNames.Clear();
        }

        public void RegisterBool(string name, bool initialValue = false)
        {
            this.ThrowIfVariableAlreadyExists(name);
            float convertedValue = initialValue == true ? 1.0f : 0.0f;
            this.Variables.Add(name, new BlackboardVariable(BlackboardVariableType.Float, convertedValue));
        }

        public void RegisterFloat(string name, float initialValue = 0.0f)
        {
            this.ThrowIfVariableAlreadyExists(name);
            this.Variables.Add(name, new BlackboardVariable(BlackboardVariableType.Float, initialValue));
        }

        public void RegisterInt(string name, int initialValue = 0)
        {
            this.ThrowIfVariableAlreadyExists(name);
            this.Variables.Add(name, new BlackboardVariable(BlackboardVariableType.Float, initialValue));
        }

        public void RegisterTrigger(string name)
        {
            this.ThrowIfVariableAlreadyExists(name);
            this.Variables.Add(name, new BlackboardVariable(BlackboardVariableType.Trigger, 0));
        }

        public void UpdateBool(string name, bool value)
        {
            this.ThrowIfVariableDoesNotExist(name);
            float convertedValue = value == true ? 1.0f : 0.0f;
            this.Variables[name].Value = convertedValue;
        }

        public void UpdateFloat(string name, float value)
        {
            this.ThrowIfVariableDoesNotExist(name);
            this.Variables[name].Value = value;
        }

        public void UpdateInt(string name, int value)
        {
            this.ThrowIfVariableDoesNotExist(name);
            this.Variables[name].Value = value;
        }

        public void ActivateTrigger(string name)
        {
            this.ThrowIfVariableDoesNotExist(name);
            this.activeTriggerNames.Add(name);
            this.Variables[name].Value = 1.0f;
        }

        public BlackboardVariable GetVariable(string name)
        {
            this.ThrowIfVariableDoesNotExist(name);
            return this.Variables[name];
        }

        private void ThrowIfVariableAlreadyExists(string name)
        {
            if(this.Variables.ContainsKey(name))
            {
                throw new ApplicationException($"variable '{name}' already exists in the blackboard");
            }
        }

        private void ThrowIfVariableDoesNotExist(string name)
        {
            if(!this.Variables.ContainsKey(name))
            {
                throw new ApplicationException($"there is no variable named '{name}' in the blackboard");
            }
        }
    }
}
