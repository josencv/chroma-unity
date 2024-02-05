namespace Chroma.Core.Infrastructure.StateMachines
{
    public class BlackboardVariable
    {
        public BlackboardVariableType Type { get; set; }
        public float Value { get; set; }

        public BlackboardVariable(BlackboardVariableType type, float value)
        {
            this.Type = type;
            this.Value = value;
        }
    }
}
