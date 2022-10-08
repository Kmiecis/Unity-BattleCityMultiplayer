using UnityEditor;
using UnityEngine;

namespace Tanks
{
    [CustomEditor(typeof(SquareObjectPainter))]
    public class HexagonObjectPainterEditor : Editor
    {
        public SquareObjectPainter Script
        {
            get => (SquareObjectPainter)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Script.IsPainting)
            {
                GUI.color = Color.green;
                if (GUILayout.Button("Stop painting"))
                {
                    Script.StopPainting();
                }
                GUI.color = Color.white;
            }
            else
            {
                if (GUILayout.Button("Start painting"))
                {
                    Script.StartPainting();
                }
            }
        }

        private void OnSceneGUI()
        {
            if (Script.IsPainting)
            {
                Script.OnSceneGUI();
            }
        }

        private void OnDisable()
        {
            Script.IsPainting = false;
        }
    }
}
