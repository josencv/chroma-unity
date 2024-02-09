using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class StateMachineWindow : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset visualTreeAsset = default;

    [MenuItem("Window/Chroma/State Machine")]
    public static void ShowExample()
    {
        StateMachineWindow wnd = GetWindow<StateMachineWindow>();
        wnd.titleContent = new GUIContent("State Machine");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = this.rootVisualElement;

        var grid = new ZoomableGrid();

        // Optionally, set its style to fill the parent or specify dimensions
        grid.style.flexGrow = 1;

        root.Add(grid);

        // Instantiate UXML
        VisualElement labelFromUXML = this.visualTreeAsset.Instantiate();
        root.Add(labelFromUXML);
    }
}
