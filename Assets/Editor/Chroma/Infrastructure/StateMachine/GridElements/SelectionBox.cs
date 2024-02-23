using Chroma.Editor.Infrastructure.StateMachine.Util;
using UnityEngine;

namespace Chroma.Editor.Infrastructure.StateMachine.GridElements
{
    public class SelectionBox : PositionedVisualElement
    {
        public SelectionBox(Vector2 position) : base(position, Vector2.one)
        {
            this.SetDefaultStyles();
        }

        protected void SetDefaultStyles()
        {
            this.style.backgroundColor = ColorUtil.FromRGBA(40, 140, 200, 40);
            this.SetBorder(ColorUtil.FromRGBA(40, 140, 200, 200), 1.0f);
        }
    }
}
