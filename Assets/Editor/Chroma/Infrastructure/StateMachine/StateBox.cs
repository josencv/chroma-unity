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
            this.style.borderTopLeftRadius = new Length(boxBorderRadius, LengthUnit.Pixel);
            this.style.borderTopRightRadius = new Length(boxBorderRadius, LengthUnit.Pixel);
            this.style.borderBottomLeftRadius = new Length(boxBorderRadius, LengthUnit.Pixel);
            this.style.borderBottomRightRadius = new Length(boxBorderRadius, LengthUnit.Pixel);
        }
    }
}
