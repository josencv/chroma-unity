using UnityEngine;
using UnityEngine.UIElements;
using USSPosition = UnityEngine.UIElements.Position;

namespace Chroma.Editor.Infrastructure.StateMachine
{
    public class StateBox : PositionedVisualElement
    {
        private const int boxBorderRadius = 6;

        public StateBox(string stateName, Vector2 position) : base(position)
        {
            this.Position = position;
            this.DefineDefaultStyles();
            this.AddLabel(stateName);
        }

        private void AddLabel(string stateName)
        {
            var label = new Label(stateName);
            label.style.unityTextAlign = TextAnchor.MiddleCenter; // Ensure text is centered
            this.Add(label); // Add the label to the box
        }

        private void DefineDefaultStyles()
        {
            this.style.width = 100;
            this.style.height = 50;
            this.style.position = USSPosition.Absolute;
            this.style.backgroundColor = Color.gray;
            this.style.justifyContent = Justify.Center;
            this.style.alignItems = Align.Center;
            this.style.borderTopLeftRadius = new Length(boxBorderRadius, LengthUnit.Pixel);
            this.style.borderTopRightRadius = new Length(boxBorderRadius, LengthUnit.Pixel);
            this.style.borderBottomLeftRadius = new Length(boxBorderRadius, LengthUnit.Pixel);
            this.style.borderBottomRightRadius = new Length(boxBorderRadius, LengthUnit.Pixel);
        }
    }
}