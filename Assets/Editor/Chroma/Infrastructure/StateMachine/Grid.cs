using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Chroma.Editor.Infrastructure.StateMachine
{
    public class Grid : VisualElement
    {
        private const int lineSpacing = 10;
        private const float minZoom = 0.2f;
        private const float maxZoom = 6.0f;
        private const float zoomSensitivity = 0.06f;
        private const int minorLinesPerMajorLine = 10;
        private const float majorLineThickness = 1.2f;
        private const float minorLineThickness = 0.8f;
        private readonly Color backgroundColor = new Color(38f / 255, 38f / 255, 38f / 255);
        private readonly Color minorLineColor = new Color(33f / 255, 33f / 255, 33f / 255);
        private readonly Color majorLineColor = new Color(30f / 255, 30f / 255, 30f / 255);

        private List<PositionedVisualElement> positionedElements = new List<PositionedVisualElement>();

        /// <summary>
        /// The logical or "simulated" position of the top-left corner of the viewport of the grid
        /// in a virtual infinite plane
        /// </summary>
        private Vector2 viewportPosition = new Vector2(1000, 1000);
        private float zoomLevel = 1.0f;
        private Vector2 currentOffset = Vector2.one;

        public Grid()
        {
            this.DefineDefaultStyles();
            this.generateVisualContent += this.DrawLines;
            this.RegisterCallback<WheelEvent>(this.OnZoom);

            this.AddStateBox();
            this.PositionStates();
        }

        private void AddStateBox()
        {
            var stateBox = new StateBox("Test State", new Vector2(this.viewportPosition.x + 100, this.viewportPosition.y + 100));
            this.positionedElements.Add(stateBox);
            this.Add(stateBox);
        }

        private void DefineDefaultStyles()
        {
            this.style.flexGrow = 1;
            this.style.position = Position.Relative;
            this.style.backgroundColor = new StyleColor(this.backgroundColor);
        }

        private void PositionStates()
        {
            foreach(PositionedVisualElement element in this.positionedElements)
            {
                // using the style values instead of `element.layout.size` because the rendered size
                // may not have been resolved yet
                // Also, this assumes the elements have a set width and height in pixel values
                float width = element.style.width.value.value;
                float height = element.style.height.value.value;

                element.style.scale = new StyleScale(new Vector2(this.zoomLevel, this.zoomLevel));
                // element.style.left = (element.Position.x - this.viewportPosition.x) * this.zoomLevel + width / 2 * (this.zoomLevel - 1);
                // element.style.top = (element.Position.y - this.viewportPosition.y) * this.zoomLevel + height / 2 * (this.zoomLevel - 1);
                element.style.left = (element.Position.x - this.viewportPosition.x) * this.zoomLevel + width / 2 * (this.zoomLevel - 1);
                element.style.top = (element.Position.y - this.viewportPosition.y) * this.zoomLevel + height / 2 * (this.zoomLevel - 1);
            }
        }

        private void OnZoom(WheelEvent evt)
        {
            // the zoom level delta depends on the zoom level itself to have a more "linear"
            // experience between zooming from zoomed-in levels vs zoomed-out ones
            float lastZoomLevel = this.zoomLevel;
            this.zoomLevel -= evt.delta.y * zoomSensitivity * this.zoomLevel;
            this.zoomLevel = Mathf.Clamp(this.zoomLevel, minZoom, maxZoom);
            this.currentOffset = this.UpdateViewportPosition(evt.localMousePosition, lastZoomLevel, this.zoomLevel, this.currentOffset, this.zoomLevel * lineSpacing * minorLinesPerMajorLine);
            this.PositionStates();
            this.MarkDirtyRepaint();
            evt.StopPropagation();
        }

        private void DrawLines(MeshGenerationContext ctx)
        {
            Painter2D painter = ctx.painter2D;
            this.DrawMajorLines(painter);
            this.DrawMinorLines(painter);
        }

        private Vector2 UpdateViewportPosition(Vector2 zoomPoint, float lastZoomLevel, float currentZoomLevel, Vector2 lastOffset, float majorLineSpacing)
        {
            float scaleFactor = currentZoomLevel / lastZoomLevel;
            // Calculate the new offset based on the zoom point and scale factor
            // The idea is to find the vector from the zoom point to the current offset,
            // scale that vector by the zoom factor, and then adjust the offset by the difference
            Vector2 scaledOffsetToZoomPoint = (zoomPoint - lastOffset) * scaleFactor;
            Vector2 newOffset = zoomPoint - scaledOffsetToZoomPoint;

            Vector2 zoomPosition = zoomPoint / currentZoomLevel;
            this.viewportPosition = this.viewportPosition + (zoomPosition * (scaleFactor - 1));

            return new Vector2(newOffset.x % majorLineSpacing, newOffset.y % majorLineSpacing);
        }

        private void DrawMajorLines(Painter2D painter)
        {
            float width = this.layout.width;
            float height = this.layout.height;
            float offsetWidth = this.layout.width - this.currentOffset.x;
            float offsetHeight = this.layout.height - this.currentOffset.y;

            // Major lines - Vertical
            painter.BeginPath();
            painter.strokeColor = this.majorLineColor;
            painter.lineWidth = majorLineThickness;
            float spaceBetweenLines = lineSpacing * this.zoomLevel * minorLinesPerMajorLine;
            for(int i = 0; i < offsetWidth / spaceBetweenLines; i++)
            {
                float nextPosition = this.currentOffset.x + i * spaceBetweenLines;
                painter.MoveTo(new Vector2(nextPosition, 0));
                painter.LineTo(new Vector2(nextPosition, height));
            }

            // Major lines - Horizontal
            for(int i = 0; i < offsetHeight / spaceBetweenLines; i++)
            {
                float nextPosition = this.currentOffset.y + i * spaceBetweenLines;
                painter.MoveTo(new Vector2(0, nextPosition));
                painter.LineTo(new Vector2(width, nextPosition));
            }

            painter.Stroke();
        }

        private void DrawMinorLines(Painter2D painter)
        {
            float width = this.layout.width;
            float height = this.layout.height;
            float offsetWidth = this.layout.width - this.currentOffset.x;
            float offsetHeight = this.layout.height - this.currentOffset.y;

            // Minor lines - Vertical
            painter.BeginPath();
            painter.strokeColor = this.minorLineColor;
            painter.lineWidth = minorLineThickness;
            float spaceBetweenLines = lineSpacing * this.zoomLevel;
            for(int i = 0; i < offsetWidth / spaceBetweenLines; i++)
            {
                if(i % minorLinesPerMajorLine == 0)
                {
                    continue;
                }

                float nextPosition = this.currentOffset.x + i * spaceBetweenLines;
                painter.MoveTo(new Vector2(nextPosition, 0));
                painter.LineTo(new Vector2(nextPosition, height));
            }

            // Minor lines - Horizontal
            for(int i = 0; i < offsetHeight / spaceBetweenLines; i++)
            {
                if(i % minorLinesPerMajorLine == 0)
                {
                    continue;
                }

                float nextPosition = this.currentOffset.y + i * spaceBetweenLines;
                painter.MoveTo(new Vector2(0, nextPosition));
                painter.LineTo(new Vector2(width, nextPosition));
            }

            painter.Stroke();
        }
    }
}