using System.Collections.Generic;
using System.Linq;
using Chroma.Core.Infrastructure.StateMachines;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Chroma.Editor.Infrastructure.StateMachine
{
    public class StateMachineWindow : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset visualTreeAsset = default;

        private IEnumerable<System.Type> stateTypes;

        private void OnEnable()
        {
            this.LoadStatesTypes();
        }

        [MenuItem("Window/Chroma/State Machine")]
        public static void ShowWindow()
        {
            StateMachineWindow wnd = GetWindow<StateMachineWindow>();
            wnd.titleContent = new GUIContent("State Machine");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = this.rootVisualElement;

            root.Add(new Grid());

            // Instantiate UXML
            VisualElement labelFromUXML = this.visualTreeAsset.Instantiate();
            root.Add(labelFromUXML);
        }

        private void LoadStatesTypes()
        {
            this.stateTypes = TypeCache.GetTypesDerivedFrom<State>().Where(t => !t.IsAbstract);
        }
    }
}