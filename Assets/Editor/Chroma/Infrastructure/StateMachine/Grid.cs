using System;
using System.Collections.Generic;
using Chroma.Editor.Infrastructure.StateMachine.GridElements;
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
        private const int lineSpacing = 10; // in units
        private const float minZoom = 0.15f;
        private const float maxZoom = 2.6f;
        private const float zoomSensitivity = 0.06f;
        private const int minorLinesPerMajorLine = 10;
        private const float majorLineThickness = 1.2f;
        private const float minorLineThickness = 0.8f;
        private const MouseButton panMouseButton = MouseButton.Left;
        private const MouseButton selectElementButton = MouseButton.Left;
        private readonly Color backgroundColor = new Color(38f / 255, 38f / 255, 38f / 255);
        private readonly Color minorLineColor = new Color(33f / 255, 33f / 255, 33f / 255);
        private readonly Color majorLineColor = new Color(30f / 255, 30f / 255, 30f / 255);

        private List<PositionedVisualElement> positionedElements = new List<PositionedVisualElement>();
        private List<SelectedElementInfo> selectedElementsInfos = new List<SelectedElementInfo>();

        /// <summary>
        /// The logical or "simulated" position of the top-left corner of the viewport of the grid
        /// in a virtual infinite plane
        /// </summary>
        private Vector2 viewportPosition = Vector2.zero;
        private float zoomLevel = 1.4f;
        private bool panning = false;
        private bool selecting = false;
        private bool movingElements = false;
        private Vector2 pointerStartPosition = Vector2.zero;
        private SelectionBox selectionBox;

        public Grid()
        {
            this.DefineDefaultStyles();
            this.AddManipulator(new ContextualMenuManipulator(this.BuildContextMenu));
            this.generateVisualContent += this.DrawLines;
            this.RegisterCallback<WheelEvent>(this.OnWheelEvent);
            this.RegisterCallback<PointerDownEvent>(this.OnPointerDown);
            this.RegisterCallback<PointerMoveEvent>(this.OnPointerMove);
            this.RegisterCallback<PointerUpEvent>(this.OnPointerUp);

            this.AddDefaultBoxes();
            this.PositionElements();
            this.InitializeSelectionBox();
        }

        private void InitializeSelectionBox()
        {
            this.selectionBox = new SelectionBox(Vector2.zero);
            this.selectionBox.Hide();
            this.positionedElements.Add(this.selectionBox);
            this.Add(this.selectionBox);
        }

        private void AddDefaultBoxes()
        {
            var state = new StateBox("Green", new Vector2(10, 10), BoxColorData.Schemes[BoxColor.Green]);
            this.positionedElements.Add(state);
            this.Add(state);

            state = new StateBox("Teal", new Vector2(210, 10), BoxColorData.Schemes[BoxColor.Teal]);
            this.positionedElements.Add(state);
            this.Add(state);

            state = new StateBox("Cyan", new Vector2(410, 10), BoxColorData.Schemes[BoxColor.Cyan]);
            this.positionedElements.Add(state);
            this.Add(state);

            state = new StateBox("Blue", new Vector2(10, 90), BoxColorData.Schemes[BoxColor.Blue]);
            this.positionedElements.Add(state);
            this.Add(state);

            state = new StateBox("Purple", new Vector2(210, 90), BoxColorData.Schemes[BoxColor.Purple]);
            this.positionedElements.Add(state);
            this.Add(state);

            state = new StateBox("Burgundy", new Vector2(410, 90), BoxColorData.Schemes[BoxColor.Burgundy]);
            this.positionedElements.Add(state);
            this.Add(state);

            state = new StateBox("Pink", new Vector2(10, 170), BoxColorData.Schemes[BoxColor.Pink]);
            this.positionedElements.Add(state);
            this.Add(state);

            state = new StateBox("Red", new Vector2(210, 170), BoxColorData.Schemes[BoxColor.Red]);
            this.positionedElements.Add(state);
            this.Add(state);

            state = new StateBox("Orange", new Vector2(410, 170), BoxColorData.Schemes[BoxColor.Orange]);
            this.positionedElements.Add(state);
            this.Add(state);

            state = new StateBox("Yellow", new Vector2(10, 250), BoxColorData.Schemes[BoxColor.Yellow]);
            this.positionedElements.Add(state);
            this.Add(state);

            state = new StateBox("Black", new Vector2(210, 250), BoxColorData.Schemes[BoxColor.Black]);
            this.positionedElements.Add(state);
            this.Add(state);

            state = new StateBox("Gray", new Vector2(410, 250), BoxColorData.Schemes[BoxColor.Gray]);
            this.positionedElements.Add(state);
            this.Add(state);

            state = new StateBox("Brown", new Vector2(10, 330), BoxColorData.Schemes[BoxColor.Brown]);
            this.positionedElements.Add(state);
            this.Add(state);
        }

        private void AddStateBox(Vector2 logicalPosition)
        {
            var stateBox = new StateBox("New State", logicalPosition, BoxColorData.Schemes[BoxColor.Purple]);
            this.positionedElements.Add(stateBox);
            stateBox.DeleteRequested += this.Remove;
            this.Add(stateBox);
            this.PositionElement(stateBox);
            this.MarkDirtyRepaint();
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
            if(this.IsMouseButtonPressed(evt.pressedButtons, panMouseButton) && evt.altKey)
            {
                this.panning = true;
            }
            else if(this.IsMouseButtonPressed(evt.pressedButtons, selectElementButton))
            {
                if(evt.target is StateBox)
                {
                    this.OnStateBoxClicked(evt, evt.target as StateBox);
                }
                else if(evt.target == this)
                {
                    this.DeselectAll();
                    this.StartMultiSelection(evt.localPosition);
                }
            }
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            if(!evt.altKey)
            {
                this.panning = false;
            }

            if(this.panning)
            {
                this.Pan(evt.deltaPosition);
            }
            else if(this.selecting)
            {
                this.UpdateSelectionBox(evt.localPosition);
            }
            else if(this.movingElements && this.selectedElementsInfos.Count > 0)
            {
                this.MoveElements(evt.localPosition);
            }
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            if(!this.IsMouseButtonPressed(evt.pressedButtons, panMouseButton))
            {
                this.panning = false;
            }

            if(!this.IsMouseButtonPressed(evt.pressedButtons, selectElementButton))
            {
                this.StopMultiSelection();
                this.StopTrackingElementMove();
            }
        }

        private void PositionElements()
        {
            foreach(PositionedVisualElement element in this.positionedElements)
            {
                this.PositionElement(element);
            }
        }

        private void OnStateBoxClicked(PointerDownEvent evt, StateBox box)
        {
            if(this.selectedElementsInfos.Find((elementInfo) => elementInfo.Element == box) == null)
            {
                if(!evt.shiftKey && !evt.ctrlKey)
                {
                    this.DeselectAll();
                }

                this.selectedElementsInfos.Add(new SelectedElementInfo(box));
                box.Select();
                box.BringToFront();
            }

            this.StartTrackingElementMove(evt);
        }

        private void DeselectAll()
        {
            foreach(SelectedElementInfo info in this.selectedElementsInfos)
            {
                info.Element.Deselect();
            }

            this.selectedElementsInfos.Clear();
        }

        private void PositionElement(PositionedVisualElement element)
        {
            // Since the scaling involves repositioning relative to the viewport, I chose not to move the logic inside the elements
            element.style.scale = new StyleScale(new Vector2(this.zoomLevel, this.zoomLevel));
            element.style.left = (element.Position.x - this.viewportPosition.x) * this.zoomLevel + element.Size.x / 2 * (this.zoomLevel - 1);
            element.style.top = (element.Position.y - this.viewportPosition.y) * this.zoomLevel + element.Size.y / 2 * (this.zoomLevel - 1);
        }

        private void StartTrackingElementMove(PointerDownEvent evt)
        {
            this.pointerStartPosition = this.LocalMousePositionToLogicalPosition(evt.localPosition);
            foreach(SelectedElementInfo elementInfo in this.selectedElementsInfos)
            {
                elementInfo.InitialPosition = elementInfo.Element.Position;
                elementInfo.Element.BringToFront();
            }

            this.movingElements = true;
        }

        private void MoveElements(Vector2 localPointerPosition)
        {
            Vector2 pointerPosition = this.LocalMousePositionToLogicalPosition(localPointerPosition);
            Vector2 dragDistance = pointerPosition - this.pointerStartPosition;

            foreach(SelectedElementInfo info in this.selectedElementsInfos)
            {
                Vector2 elementVirtualPosition = info.InitialPosition + dragDistance;
                Vector2 closestSnapPosition = this.SnapToGrid(elementVirtualPosition, new Vector2(lineSpacing, lineSpacing));
                info.Element.Position = closestSnapPosition;
                this.PositionElement(info.Element);
            }

            this.MarkDirtyRepaint();
        }

        private void StopTrackingElementMove()
        {
            this.movingElements = false;
        }

        private void StartMultiSelection(Vector3 localMousePosition)
        {
            Vector2 pointerLogicalPosition = this.LocalMousePositionToLogicalPosition(localMousePosition);
            this.selecting = true;
            this.pointerStartPosition = pointerLogicalPosition;
            this.selectionBox.Position = pointerLogicalPosition;
            this.selectionBox.Size = Vector2.zero;
            this.selectionBox.BringToFront();
            this.selectionBox.Show();
            this.PositionElement(this.selectionBox);
            this.MarkDirtyRepaint();
        }

        private void UpdateSelectionBox(Vector2 localMousePosition)
        {
            Vector2 mouseLogicalPosition = this.LocalMousePositionToLogicalPosition(localMousePosition);
            Vector2 dragStartPosition = this.pointerStartPosition;
            Vector2 signedSize = mouseLogicalPosition - dragStartPosition;
            Vector2 size = new Vector2(math.abs(signedSize.x), math.abs(signedSize.y));
            Vector2 topLeftPosition = new Vector2(math.min(mouseLogicalPosition.x, dragStartPosition.x), math.min(mouseLogicalPosition.y, dragStartPosition.y));
            this.selectionBox.Position = topLeftPosition;
            this.selectionBox.Size = size;
            this.PositionElement(this.selectionBox);

            // This may need to be optimized
            this.selectedElementsInfos.Clear();
            foreach(PositionedVisualElement element in this.positionedElements)
            {
                element.Deselect();
                Rect selectionRect = this.selectionBox.Rect;
                if(selectionRect.Overlaps(element.Rect))
                {
                    element.Select();
                    this.selectedElementsInfos.Add(new SelectedElementInfo(element));
                }
            }

            this.MarkDirtyRepaint();
        }

        private void StopMultiSelection()
        {
            this.selecting = false;
            this.selectionBox.Hide();
        }

        private void BuildContextMenu(ContextualMenuPopulateEvent evt)
        {
            if(evt.target == this)
            {
                Vector2 logicalPosition = this.LocalMousePositionToLogicalPosition(evt.localMousePosition);
                evt.menu.AppendAction("New State", action => this.AddStateBox(logicalPosition), DropdownMenuAction.AlwaysEnabled);
            }

            evt.StopPropagation();
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

            this.PositionElements();
            this.MarkDirtyRepaint();
        }

        private void Pan(Vector2 deltaPosition)
        {
            // the pan is done contrary to the delta of the mouse, to simulate dragging the grid
            this.viewportPosition = this.viewportPosition - deltaPosition / this.zoomLevel;
            this.PositionElements();
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

        private bool IsMouseButtonPressed(int pressedButtonsBitmask, MouseButton button)
        {
            return (pressedButtonsBitmask & (1 << (int)button)) != 0;
        }

        private Vector2 SnapToGrid(Vector2 originalPosition, Vector2 gridSize)
        {
            float x = Mathf.Round(originalPosition.x / gridSize.x) * gridSize.x;
            float y = Mathf.Round(originalPosition.y / gridSize.y) * gridSize.y;

            return new Vector2(x, y);
        }

        private Vector2 LocalMousePositionToLogicalPosition(Vector2 localMousePosition)
        {
            return this.viewportPosition + localMousePosition / this.zoomLevel;
        }

        private Vector2 LocalMousePositionToLogicalPosition(Vector3 localMousePosition)
        {
            return this.LocalMousePositionToLogicalPosition((Vector2)localMousePosition);
        }
    }

    class SelectedElementInfo
    {
        public PositionedVisualElement Element { get; }
        public Vector2 InitialPosition { get; set; }

        public SelectedElementInfo(PositionedVisualElement element)
        {
            this.Element = element;
            this.InitialPosition = element.Position;
        }
    }
}