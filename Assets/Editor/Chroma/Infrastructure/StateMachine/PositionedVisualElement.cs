using UnityEngine;
using UnityEngine.UIElements;

namespace Chroma.Editor.Infrastructure.StateMachine
{
    public abstract class PositionedVisualElement : VisualElement
    {
        public Vector2 Position { get; set; }

        public PositionedVisualElement(Vector2 position)
        {
            this.Position = position;
        }
    }
}
