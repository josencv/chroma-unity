using System;
using Chroma.Editor.Infrastructure.StateMachine.Util;
using UnityEngine;
using UnityEngine.UIElements;

namespace Chroma.Editor.Infrastructure.StateMachine.GridElements
{
    public class StateBox : PositionedVisualElement
    {
        private const int boxBorderRadius = 3;

        public event Action<VisualElement> DeleteRequested;

        private float borderWidth = 2.0f;
        private Color selectionBorderColor = ColorUtils.FromRGB(54, 173, 220);
        private float selectionBorderWidth = 2.0f;
        private BoxColorScheme colorScheme;

        public StateBox(string stateName, Vector2 position, BoxColorScheme colorScheme) : base(position, new Vector2(150, 30))
        {
            this.Position = position;
            this.colorScheme = colorScheme;
            this.focusable = true;
            this.SetDefaultStyles();
            this.AddLabel(stateName);
            this.RegisterCallback<FocusEvent>(this.OnFocus);
            this.RegisterCallback<BlurEvent>(this.OnBlur);
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
            this.style.backgroundColor = this.colorScheme.BackgroundColor;
            this.style.justifyContent = Justify.Center;
            this.style.alignItems = Align.Center;
            this.style.borderTopLeftRadius = new Length(boxBorderRadius, LengthUnit.Pixel);
            this.style.borderTopRightRadius = new Length(boxBorderRadius, LengthUnit.Pixel);
            this.style.borderBottomLeftRadius = new Length(boxBorderRadius, LengthUnit.Pixel);
            this.style.borderBottomRightRadius = new Length(boxBorderRadius, LengthUnit.Pixel);
            this.SetBorder(this.colorScheme.BorderColor, this.borderWidth);
        }

        protected override void BuildContextMenu(ContextualMenuPopulateEvent evt)
        {
            if(evt.target == this)
            {
                evt.menu.AppendAction("Delete", action => this.DeleteRequested?.Invoke(this), DropdownMenuAction.AlwaysEnabled);
            }

            evt.StopPropagation();
        }

        private void OnFocus(FocusEvent evt)
        {
            this.SetBorder(this.selectionBorderColor, this.selectionBorderWidth);
        }

        private void OnBlur(BlurEvent evt)
        {
            this.SetBorder(this.colorScheme.BorderColor, this.borderWidth);
        }

        private void SetBorder(Color color, float width)
        {
            this.SetBorderColor(color);
            this.SetBorderWidth(width);
        }

        private void SetBorderColor(Color color)
        {
            this.style.borderTopColor = color;
            this.style.borderLeftColor = color;
            this.style.borderBottomColor = color;
            this.style.borderRightColor = color;
        }

        private void SetBorderWidth(float width)
        {
            this.style.borderTopWidth = width;
            this.style.borderLeftWidth = width;
            this.style.borderBottomWidth = width;
            this.style.borderRightWidth = width;
        }
    }
}
