using System;
using Chroma.Editor.Infrastructure.StateMachine.Util;
using UnityEngine;
using UnityEngine.UIElements;

namespace Chroma.Editor.Infrastructure.StateMachine.GridElements
{
    public class StateBox : PositionedVisualElement
    {
        private const int boxBorderRadius = 3;

        public event Action<PositionedVisualElement> DeleteRequested;

        private float borderWidth = 2.0f;
        private Color selectionBorderColor = ColorUtil.FromRGB(54, 173, 220);
        private float selectionBorderWidth = 2.0f;
        private BoxColorScheme colorScheme;

        public StateBox(string stateName, Vector2 position, BoxColorScheme colorScheme) : base(position, new Vector2(150, 30))
        {
            this.colorScheme = colorScheme;
            this.SetDefaultStyles();
            this.AddLabel(stateName);
        }

        private void SetDefaultStyles()
        {
            this.style.backgroundColor = this.colorScheme.BackgroundColor;
            this.style.justifyContent = Justify.Center;
            this.style.alignItems = Align.Center;
            this.style.borderTopLeftRadius = new Length(boxBorderRadius, LengthUnit.Pixel);
            this.style.borderTopRightRadius = new Length(boxBorderRadius, LengthUnit.Pixel);
            this.style.borderBottomLeftRadius = new Length(boxBorderRadius, LengthUnit.Pixel);
            this.style.borderBottomRightRadius = new Length(boxBorderRadius, LengthUnit.Pixel);
            this.SetBorder(this.colorScheme.BorderColor, this.borderWidth);
        }

        public override void Select()
        {
            base.Select();
            this.SetBorder(this.selectionBorderColor, this.selectionBorderWidth);
        }

        public override void Deselect()
        {
            base.Deselect();
            this.SetBorder(this.colorScheme.BorderColor, this.borderWidth);
        }

        protected override void BuildContextMenu(ContextualMenuPopulateEvent evt)
        {
            if(evt.target == this)
            {
                evt.menu.AppendAction("Delete", action => this.DeleteRequested?.Invoke(this), DropdownMenuAction.AlwaysEnabled);
            }
        }

        private void AddLabel(string stateName)
        {
            var label = new Label(stateName);
            label.style.unityTextAlign = TextAnchor.MiddleCenter; // Ensure text is centered
            this.Add(label); // Add the label to the box
        }
    }
}
