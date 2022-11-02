using Tanks;
using UnityEditor;
using UnityEngine;

namespace TanksEditor
{
    [CustomEditor(typeof(DebugEventHandler))]
    public class DebugEventHandlerEditor : Editor
    {
        public DebugEventHandler Script
        {
            get => (DebugEventHandler)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Raise"))
            {
                Script.OnRaise();
            }
        }
    }
}
