using UnityEngine;
using UnityEngine.UIElements;

namespace Chroma.Editor.Infrastructure.StateMachine
{
    public class StateBox : PositionedVisualElement
    {
        private const int boxBorderRadius = 3;

        public StateBox(string stateName, Vector2 position) : base(position, new Vector2(150, 30))
        {
            this.Position = position;
            this.SetDefaultStyles();
            this.AddLabel(stateName);
            this.focusable = true;
        }

        private void AddLabel(string stateName)
        {
            var label = new Label(stateName);
            label.style.unityTextAlign = TextAnchor.MiddleCenter; // Ensure text is centered
            this.Add(label); // Add the label to the box
        }

        protected override void SetDefaultStyles()
        {
            base.SetDefaultStyles();
            this.style.backgroundColor = new Color(10.0f / 255, 140.0f / 255, 70.0f / 255);
            this.style.justifyContent = Justify.Center;
            this.style.alignItems = Align.Center;
            this.style.borderTopWidth = 1.0f;
            this.style.borderLeftWidth = 1.0f;
            this.style.borderBottomWidth = 1.0f;
            this.style.borderRightWidth = 1.0f;
            this.style.borderTopColor = Color.black;
            this.style.borderLeftColor = Color.black;
            this.style.borderBottomColor = Color.black;
            this.style.borderRightColor = Color.black;
            this.style.borderTopLeftRadius = new Length(boxBorderRadius, LengthUnit.Pixel);
            this.style.borderTopRightRadius = new Length(boxBorderRadius, LengthUnit.Pixel);
            this.style.borderBottomLeftRadius = new Length(boxBorderRadius, LengthUnit.Pixel);
            this.style.borderBottomRightRadius = new Length(boxBorderRadius, LengthUnit.Pixel);
        }
    }
}
