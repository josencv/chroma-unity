namespace Chroma.Core.Infrastructure.StateMachines
{
    public class EmptyState : State
    {
        protected EmptyState(string name) : base(name)
        {

        }

        public override void OnEnter()
        {

        }

        public override void OnExit()
        {

        }

        public override void Tick(float deltaTime)
        {

        }
    }
}
