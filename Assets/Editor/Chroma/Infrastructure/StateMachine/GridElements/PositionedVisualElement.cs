using UnityEngine;
using UnityEngine.UIElements;
using USSPosition = UnityEngine.UIElements.Position;

namespace Chroma.Editor.Infrastructure.StateMachine.GridElements
{
    public abstract class PositionedVisualElement : VisualElement
    {
        private Vector2 size;
        public Vector2 Position { get; set; }
        public Vector2 Size
        {
            get { return this.size; }
            set
            {
                this.size = value;
                this.style.width = this.Size.x;
                this.style.height = this.Size.y;
            }
        }

        /// <summary>
        /// Returns the logical rect of the element
        /// </summary>
        public Rect Rect
        {
            get { return new Rect(this.Position, this.Size); }
        }

        public PositionedVisualElement(Vector2 position, Vector2 size)
        {
            this.Position = position;
            this.size = size;

            this.SetDefaultStyles();

            // Hack to prevent the grid from receiving click events when clicking on positioned
            // elements. There is probably a clean way to do it, but I haven't found it yet.
            this.AddManipulator(new ContextualMenuManipulator(this.BuildContextMenu));
        }

        public virtual void Select() { }

        public virtual void Deselect() { }

        private void SetDefaultStyles()
        {
            this.style.display = DisplayStyle.Flex;
            this.style.position = USSPosition.Absolute;
            this.style.width = this.Size.x;
            this.style.height = this.Size.y;
        }

        protected void SetBorder(Color color, float width)
        {
            this.SetBorderColor(color);
            this.SetBorderWidth(width);
        }

        protected void SetBorderColor(Color color)
        {
            this.style.borderTopColor = color;
            this.style.borderLeftColor = color;
            this.style.borderBottomColor = color;
            this.style.borderRightColor = color;
        }

        protected void SetBorderWidth(float width)
        {
            this.style.borderTopWidth = width;
            this.style.borderLeftWidth = width;
            this.style.borderBottomWidth = width;
            this.style.borderRightWidth = width;
        }

        public void Show()
        {
            this.visible = true;
        }

        public void Hide()
        {
            this.visible = false;
        }

        protected virtual void BuildContextMenu(ContextualMenuPopulateEvent evt) { }
    }
}
