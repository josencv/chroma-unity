using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

namespace Chroma.Editor.Infrastructure.StateMachine
{
    [Flags]
    enum MouseButton
    {
        Left = 0,
        Right = 1,
        Middle = 2
    }

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
        private const MouseButton panMouseButton = MouseButton.Left;
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
        private bool panning = false;

        public Grid()
        {
            this.DefineDefaultStyles();
            this.generateVisualContent += this.DrawLines;
            this.RegisterCallback<WheelEvent>(this.OnWheelEvent);
            this.RegisterCallback<PointerDownEvent>(this.OnPointerDown);
            this.RegisterCallback<PointerMoveEvent>(this.OnPointerMove);
            this.RegisterCallback<PointerUpEvent>(this.OnPointerUp);

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

        private void OnWheelEvent(WheelEvent evt)
        {
            this.Zoom(evt.localMousePosition, evt.delta.y);
            evt.StopPropagation();
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            if(this.isMouseButtonPressed(evt.pressedButtons, panMouseButton) && evt.altKey)
            {
                this.panning = true;
            }

            evt.StopPropagation();
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            if(!evt.altKey)
            {
                this.panning = false;
                return;
                // Apply panning based on evt.position delta
            }

            if(this.panning)
            {
                this.Pan(evt.deltaPosition);
            }

            evt.StopPropagation();
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            if(this.isMouseButtonPressed(evt.pressedButtons, panMouseButton))
            {
                this.panning = false;
            }
        }

        private void PositionStates()
        {
            foreach(PositionedVisualElement element in this.positionedElements)
            {
                // Since the scaling involves repositioning relative to the viewport, I chose not to move the logic inside the elements
                float width = element.style.width.value.value;
                float height = element.style.height.value.value;
                element.style.scale = new StyleScale(new Vector2(this.zoomLevel, this.zoomLevel));
                element.style.left = (element.Position.x - this.viewportPosition.x) * this.zoomLevel + width / 2 * (this.zoomLevel - 1);
                element.style.top = (element.Position.y - this.viewportPosition.y) * this.zoomLevel + height / 2 * (this.zoomLevel - 1);
            }
        }

        private void Zoom(Vector2 zoomPoint, float zoomDelta)
        {
            // the zoom level delta depends on the zoom level itself to have a more "linear"
            // experience between zooming from zoomed-in levels vs zoomed-out ones
            float lastZoomLevel = this.zoomLevel;
            this.zoomLevel -= zoomDelta * zoomSensitivity * this.zoomLevel;
            this.zoomLevel = Mathf.Clamp(this.zoomLevel, minZoom, maxZoom);

            float scaleFactor = this.zoomLevel / lastZoomLevel;
            Vector2 zoomPosition = zoomPoint / this.zoomLevel;
            this.viewportPosition = this.viewportPosition + (zoomPosition * (scaleFactor - 1));

            this.PositionStates();
            this.MarkDirtyRepaint();
        }

        private void Pan(Vector2 deltaPosition)
        {
            // the pan is done contrary to the delta of the mouse, to simulate dragging the grid
            this.viewportPosition = this.viewportPosition - deltaPosition / this.zoomLevel;
            this.PositionStates();
            this.MarkDirtyRepaint();
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

        private bool isMouseButtonPressed(int pressedButtonsBitmask, MouseButton button)
        {
            return (pressedButtonsBitmask & (1 << (int)button)) != 0;
        }
    }
}