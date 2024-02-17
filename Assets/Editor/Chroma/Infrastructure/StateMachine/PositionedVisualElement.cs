using UnityEngine;
using UnityEngine.UIElements;
using USSPosition = UnityEngine.UIElements.Position;

namespace Chroma.Editor.Infrastructure.StateMachine
{
    public abstract class PositionedVisualElement : VisualElement
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }

        public PositionedVisualElement(Vector2 position, Vector2 size)
        {
            this.Position = position;
            this.Size = size;

            // Hack to prevent the grid from receiving click events when clicking on positioned
            // elements. There is probably a clean way to do it, but I haven't found it yet.
            this.AddManipulator(new ContextualMenuManipulator(this.BuildContextMenu));
        }

        protected virtual void SetDefaultStyles()
        {
            this.style.display = DisplayStyle.Flex;
            this.style.position = USSPosition.Absolute;
            this.style.width = this.Size.x;
            this.style.height = this.Size.y;
        }

        public void Show()
        {
            this.visible = true;
        }

        public void Hide()
        {
            this.visible = false;
        }


        protected void BuildContextMenu(ContextualMenuPopulateEvent evt)
        {
            return;
        }
    }
}
