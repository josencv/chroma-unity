using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Chroma.Editor.Infrastructure.StateMachine
{
    public class Grid : VisualElement
    {
        // For simplicity, the unit is equal to 1 pixel with a zoom level of 1
        private const int lineSpacing = 14; // in units
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
        private Vector2 viewportPosition = Vector2.zero;
        private float zoomLevel = 1.4f;

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
            var stateBox = new StateBox("Test State", new Vector2(100, 150));
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
            this.UpdateViewportPosition(evt.localMousePosition, lastZoomLevel, this.zoomLevel);
            this.PositionStates();
            this.MarkDirtyRepaint();
            evt.StopPropagation();
        }

        private void UpdateViewportPosition(Vector2 zoomPoint, float lastZoomLevel, float currentZoomLevel)
        {
            float scaleFactor = currentZoomLevel / lastZoomLevel;
            Vector2 zoomPosition = zoomPoint / currentZoomLevel;
            this.viewportPosition = this.viewportPosition + (zoomPosition * (scaleFactor - 1));
        }

        private void DrawLines(MeshGenerationContext ctx)
        {
            Painter2D painter = ctx.painter2D;
            this.DrawMinorLines(painter);
            this.DrawMajorLines(painter);
        }

        private void DrawMajorLines(Painter2D painter)
        {
            this.DrawLines(painter, this.majorLineColor, majorLineThickness, lineSpacing * minorLinesPerMajorLine);
        }

        private void DrawMinorLines(Painter2D painter)
        {
            this.DrawLines(painter, this.minorLineColor, minorLineThickness, lineSpacing);
        }

        private void DrawLines(Painter2D painter, Color lineColor, float lineThickness, float unitsBetweenLines)
        {
            float pixelsBetweenLines = unitsBetweenLines * this.zoomLevel;
            Vector2 viewportSize = this.layout.size;
            Vector2 drawOffset = new Vector2(
                (unitsBetweenLines - this.viewportPosition.x % unitsBetweenLines) % unitsBetweenLines,
                (unitsBetweenLines - this.viewportPosition.y % unitsBetweenLines) % unitsBetweenLines
                ) * this.zoomLevel;

            painter.BeginPath();
            painter.strokeColor = lineColor;
            painter.lineWidth = lineThickness;
            for(int i = 0; i < viewportSize.x / pixelsBetweenLines; i++)
            {
                float nextPosition = drawOffset.x + i * pixelsBetweenLines;
                painter.MoveTo(new Vector2(nextPosition, 0));
                painter.LineTo(new Vector2(nextPosition, viewportSize.y));
            }

            // Major lines - Horizontal
            for(int i = 0; i < viewportSize.y / pixelsBetweenLines; i++)
            {
                float nextPosition = drawOffset.y + i * pixelsBetweenLines;
                painter.MoveTo(new Vector2(0, nextPosition));
                painter.LineTo(new Vector2(viewportSize.x, nextPosition));
            }

            painter.Stroke();
        }
    }
}