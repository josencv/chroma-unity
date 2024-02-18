
using UnityEngine;

namespace Chroma.Editor.Infrastructure.StateMachine.GridElements
{
    public struct BoxColorScheme
    {
        public Color BackgroundColor { get; }
        public Color BorderColor { get; }

        public BoxColorScheme(Color backgroundColor, Color borderColor)
        {
            this.BackgroundColor = backgroundColor;
            this.BorderColor = borderColor;
        }
    }
}
