#if UNITY_EDITOR
using CodeStage.AdvancedFPSCounter.Editor.UI;
using CodeStage.AdvancedFPSCounter.Utils;
using UnityEditor;
using UnityEngine;

namespace CodeStage.AdvancedFPSCounter
{
	[CustomEditor(typeof(AFPSRenderRecorder))]
	[CanEditMultipleObjects()]
	public class AFPSRenderRecorderEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();
			EditorUIUtils.SetupStyles();
			GUILayout.Label("This component is used by <b>Advanced FPS Counter</b> to measure camera <b>Render Time</b>.", EditorUIUtils.richMiniLabel);
		}
	}
}
#endif