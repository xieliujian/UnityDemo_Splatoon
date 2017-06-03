#if UNITY_EDITOR
using CodeStage.AdvancedFPSCounter.Editor.UI;
using CodeStage.AdvancedFPSCounter.Labels;
using CodeStage.AdvancedFPSCounter.Utils;
using UnityEditor;
using UnityEngine;

namespace CodeStage.AdvancedFPSCounter
{
	[CustomEditor(typeof(AFPSCounter))]
	internal class AFPSCounterEditor: UnityEditor.Editor
	{
		private AFPSCounter me;

		private SerializedProperty operationMode;
		private SerializedProperty fps;
		private SerializedProperty fpsEnabled;
		private SerializedProperty fpsAnchor;
		private SerializedProperty fpsInterval;
		private SerializedProperty fpsMilliseconds;

		private SerializedProperty fpsAverage;
		private SerializedProperty fpsAverageMilliseconds;
		private SerializedProperty fpsAverageNewLine;
		private SerializedProperty fpsAverageSamples;
		private SerializedProperty fpsResetAverageOnNewScene;

		private SerializedProperty fpsMinMax;
		private SerializedProperty fpsMinMaxMilliseconds;
		private SerializedProperty fpsMinMaxNewLine;
		private SerializedProperty fpsMinMaxTwoLines;
		private SerializedProperty fpsResetMinMaxOnNewScene;
		private SerializedProperty fpsMinMaxIntervalsToSkip;

		private SerializedProperty fpsRender;
		private SerializedProperty fpsRenderNewLine;
		private SerializedProperty fpsRenderAutoAdd;

		private SerializedProperty fpsWarningLevelValue;
		private SerializedProperty fpsCriticalLevelValue;
		private SerializedProperty fpsColor;
		private SerializedProperty fpsColorWarning;
		private SerializedProperty fpsColorCritical;
		private SerializedProperty fpsColorRender;
		private SerializedProperty fpsStyle;

		private SerializedProperty memory;
		private SerializedProperty memoryEnabled;
		private SerializedProperty memoryAnchor;
	    private SerializedProperty memoryColor;
	    private SerializedProperty memoryStyle;
	    private SerializedProperty memoryInterval;
	    private SerializedProperty memoryPrecise;
	    private SerializedProperty memoryTotal;
	    private SerializedProperty memoryAllocated;
	    private SerializedProperty memoryMonoUsage;

	    private SerializedProperty device;
		private SerializedProperty deviceEnabled;
		private SerializedProperty deviceAnchor;
	    private SerializedProperty deviceColor;
	    private SerializedProperty deviceStyle;
	    private SerializedProperty deviceCpuModel;
		private SerializedProperty devicePlatform;
		private SerializedProperty deviceGpuModel;
		private SerializedProperty deviceGpuApi;
		private SerializedProperty deviceGpuSpec;
		private SerializedProperty deviceRamSize;
		private SerializedProperty deviceScreenData;
		private SerializedProperty deviceModel;

		private SerializedProperty lookAndFeelFoldout;
		private SerializedProperty autoScale;
		private SerializedProperty scaleFactor;
		private SerializedProperty labelsFont;
		private SerializedProperty fontSize;
		private SerializedProperty lineSpacing;
		private SerializedProperty countersSpacing;
		private SerializedProperty paddingOffset;
		private SerializedProperty pixelPerfect;

		private SerializedProperty background;
		private SerializedProperty backgroundColor;
		private SerializedProperty backgroundPadding;

		private SerializedProperty shadow;
		private SerializedProperty shadowColor;
		private SerializedProperty shadowDistance;

		private SerializedProperty outline;
		private SerializedProperty outlineColor;
		private SerializedProperty outlineDistance;

		private SerializedProperty advancedFoldout;
		private SerializedProperty sortingOrder;

		private SerializedProperty hotKey;
		private SerializedProperty hotKeyCtrl;
		private SerializedProperty hotKeyShift;
		private SerializedProperty hotKeyAlt;
		private SerializedProperty circleGesture;
		private SerializedProperty keepAlive;
		private SerializedProperty forceFrameRate;
		private SerializedProperty forcedFrameRate;

		private LabelAnchor groupAnchor = LabelAnchor.UpperLeft;

		public void OnEnable()
		{
			me = target as AFPSCounter;

			operationMode = serializedObject.FindProperty("operationMode");

			hotKey = serializedObject.FindProperty("hotKey");
			hotKeyCtrl = serializedObject.FindProperty("hotKeyCtrl");
			hotKeyShift = serializedObject.FindProperty("hotKeyShift");
			hotKeyAlt = serializedObject.FindProperty("hotKeyAlt");
            circleGesture = serializedObject.FindProperty("circleGesture");
			keepAlive = serializedObject.FindProperty("keepAlive");
			forceFrameRate = serializedObject.FindProperty("forceFrameRate");
			forcedFrameRate = serializedObject.FindProperty("forcedFrameRate");

			lookAndFeelFoldout = serializedObject.FindProperty("lookAndFeelFoldout");
            autoScale = serializedObject.FindProperty("autoScale");
			scaleFactor = serializedObject.FindProperty("scaleFactor");
			labelsFont = serializedObject.FindProperty("labelsFont");
			fontSize = serializedObject.FindProperty("fontSize");
			lineSpacing = serializedObject.FindProperty("lineSpacing");
			countersSpacing = serializedObject.FindProperty("countersSpacing");
			paddingOffset = serializedObject.FindProperty("paddingOffset");
            pixelPerfect = serializedObject.FindProperty("pixelPerfect");

			background = serializedObject.FindProperty("background");
			backgroundColor = serializedObject.FindProperty("backgroundColor");
            backgroundPadding = serializedObject.FindProperty("backgroundPadding");

			shadow = serializedObject.FindProperty("shadow");
			shadowColor = serializedObject.FindProperty("shadowColor");
			shadowDistance = serializedObject.FindProperty("shadowDistance");

			outline = serializedObject.FindProperty("outline");
			outlineColor = serializedObject.FindProperty("outlineColor");
			outlineDistance = serializedObject.FindProperty("outlineDistance");

			advancedFoldout = serializedObject.FindProperty("advancedFoldout");
			sortingOrder = serializedObject.FindProperty("sortingOrder");

			fps = serializedObject.FindProperty("fpsCounter");
			fpsEnabled = fps.FindPropertyRelative("enabled");
			fpsInterval = fps.FindPropertyRelative("updateInterval");
			fpsAnchor = fps.FindPropertyRelative("anchor");
		    fpsMilliseconds = fps.FindPropertyRelative("milliseconds");

		    fpsAverage = fps.FindPropertyRelative("average");
		    fpsAverageMilliseconds = fps.FindPropertyRelative("averageMilliseconds");
		    fpsAverageNewLine = fps.FindPropertyRelative("averageNewLine");
		    fpsAverageSamples = fps.FindPropertyRelative("averageSamples");
		    fpsResetAverageOnNewScene = fps.FindPropertyRelative("resetAverageOnNewScene");

			fpsMinMax = fps.FindPropertyRelative("minMax");
		    fpsMinMaxMilliseconds = fps.FindPropertyRelative("minMaxMilliseconds");
		    fpsMinMaxNewLine = fps.FindPropertyRelative("minMaxNewLine");
		    fpsMinMaxTwoLines = fps.FindPropertyRelative("minMaxTwoLines");
		    fpsResetMinMaxOnNewScene = fps.FindPropertyRelative("resetMinMaxOnNewScene");
		    fpsMinMaxIntervalsToSkip = fps.FindPropertyRelative("minMaxIntervalsToSkip");

			fpsRender = fps.FindPropertyRelative("render");
			fpsRenderNewLine = fps.FindPropertyRelative("renderNewLine");
			fpsRenderAutoAdd = fps.FindPropertyRelative("renderAutoAdd");

		    fpsWarningLevelValue = fps.FindPropertyRelative("warningLevelValue");
		    fpsCriticalLevelValue = fps.FindPropertyRelative("criticalLevelValue");
		    fpsColor = fps.FindPropertyRelative("color");
		    fpsColorWarning = fps.FindPropertyRelative("colorWarning");
		    fpsColorCritical = fps.FindPropertyRelative("colorCritical");
			fpsColorRender = fps.FindPropertyRelative("colorRender");
		    fpsStyle = fps.FindPropertyRelative("style");

		    memory = serializedObject.FindProperty("memoryCounter");
			memoryEnabled = memory.FindPropertyRelative("enabled");
			memoryInterval = memory.FindPropertyRelative("updateInterval");
			memoryAnchor = memory.FindPropertyRelative("anchor");
		    memoryPrecise = memory.FindPropertyRelative("precise");
		    memoryColor = memory.FindPropertyRelative("color");
            memoryStyle = memory.FindPropertyRelative("style");
		    memoryTotal = memory.FindPropertyRelative("total");
			memoryAllocated = memory.FindPropertyRelative("allocated");
			memoryMonoUsage = memory.FindPropertyRelative("monoUsage");

			device = serializedObject.FindProperty("deviceInfoCounter");
			deviceEnabled = device.FindPropertyRelative("enabled");
			deviceAnchor = device.FindPropertyRelative("anchor");
		    deviceColor = device.FindPropertyRelative("color");
            deviceStyle = device.FindPropertyRelative("style");
		    devicePlatform = device.FindPropertyRelative("platform");
			deviceCpuModel = device.FindPropertyRelative("cpuModel");
			deviceGpuModel = device.FindPropertyRelative("gpuModel");
			deviceGpuApi = device.FindPropertyRelative("gpuApi");
			deviceGpuSpec = device.FindPropertyRelative("gpuSpec");
			deviceRamSize = device.FindPropertyRelative("ramSize");
			deviceScreenData = device.FindPropertyRelative("screenData");
            deviceModel = device.FindPropertyRelative("deviceModel");
		}

		public override void OnInspectorGUI()
		{
			if (me == null) return;
			serializedObject.Update();

			EditorUIUtils.SetupStyles();

			GUILayout.Space(5);

			EditorUIUtils.DrawProperty(operationMode, () => me.OperationMode = (OperationMode)operationMode.enumValueIndex);

			EditorGUILayout.PropertyField(hotKey);

			using (EditorUIUtils.Horizontal())
			{
				GUILayout.FlexibleSpace();

				EditorGUIUtility.labelWidth = 70;
				EditorGUILayout.PropertyField(hotKeyCtrl, new GUIContent("Ctrl / Cmd", hotKeyCtrl.tooltip), GUILayout.Width(85));

				EditorGUIUtility.labelWidth = 20;
				EditorGUILayout.PropertyField(hotKeyAlt, new GUIContent("Alt", hotKeyAlt.tooltip), GUILayout.Width(35));

				EditorGUIUtility.labelWidth = 35;
				EditorGUILayout.PropertyField(hotKeyShift, new GUIContent("Shift", hotKeyShift.tooltip), GUILayout.Width(50));
			}
			EditorGUIUtility.labelWidth = 0;

            EditorGUILayout.PropertyField(circleGesture);

            EditorGUILayout.PropertyField(keepAlive);
			if (me.transform.parent != null)
			{
				EditorGUILayout.LabelField("Keep Alive option will keep alive root level object (" + me.transform.root.name + ")!", EditorStyles.wordWrappedMiniLabel);
			}

			using (EditorUIUtils.Horizontal(GUILayout.ExpandWidth(true)))
			{
				EditorUIUtils.DrawProperty(forceFrameRate, "Force FPS", () => me.ForceFrameRate = forceFrameRate.boolValue);
				EditorUIUtils.DrawProperty(forcedFrameRate, GUIContent.none, () => me.ForcedFrameRate = forcedFrameRate.intValue);
			}

			if (EditorUIUtils.Foldout(lookAndFeelFoldout, "Look & Feel"))
			{
				EditorUIUtils.Indent();

				EditorUIUtils.DrawProperty(autoScale, () => me.AutoScale = autoScale.boolValue);

			    if (autoScale.boolValue)
			    {
			        GUI.enabled = false;
			    }
				EditorUIUtils.DrawProperty(scaleFactor, () => me.ScaleFactor = scaleFactor.floatValue);
                GUI.enabled = true;
                EditorUIUtils.DrawProperty(labelsFont, () => me.LabelsFont = (Font)labelsFont.objectReferenceValue);
				EditorUIUtils.DrawProperty(fontSize, () => me.FontSize = fontSize.intValue);
				EditorUIUtils.DrawProperty(lineSpacing, () => me.LineSpacing = lineSpacing.floatValue);
				EditorUIUtils.DrawProperty(countersSpacing, () => me.CountersSpacing = countersSpacing.intValue);
				EditorUIUtils.DrawProperty(paddingOffset, () => me.PaddingOffset = paddingOffset.vector2Value);
				EditorUIUtils.DrawProperty(pixelPerfect, () => me.PixelPerfect = pixelPerfect.boolValue);

				EditorUIUtils.Header("Effects");
				EditorUIUtils.Separator();

				EditorUIUtils.DrawProperty(background, () => me.Background = background.boolValue);
				if (background.boolValue)
				{
					EditorUIUtils.Indent();
					EditorUIUtils.DrawProperty(backgroundColor, "Color", () => me.BackgroundColor = backgroundColor.colorValue);
					EditorUIUtils.DrawProperty(backgroundPadding, "Padding", () => me.BackgroundPadding = backgroundPadding.intValue);
					EditorUIUtils.UnIndent();
				}

				EditorUIUtils.DrawProperty(shadow, () => me.Shadow = shadow.boolValue);
				if (shadow.boolValue)
				{
					EditorUIUtils.Indent();
                    EditorUIUtils.DrawProperty(shadowColor, () => me.ShadowColor = shadowColor.colorValue);
					EditorUIUtils.DrawProperty(shadowDistance, () => me.ShadowDistance = shadowDistance.vector2Value);
                    EditorGUILayout.LabelField(new GUIContent("<b>This effect is resource-heavy</b>", "Such effect increases resources usage on each text refresh."), EditorUIUtils.richMiniLabel);
                    EditorUIUtils.UnIndent();
				}

				EditorUIUtils.DrawProperty(outline, () => me.Outline = outline.boolValue);
				if (outline.boolValue)
				{
					EditorUIUtils.Indent();
					EditorUIUtils.DrawProperty(outlineColor, () => me.OutlineColor = outlineColor.colorValue);
					EditorUIUtils.DrawProperty(outlineDistance, () => me.OutlineDistance = outlineDistance.vector2Value);
                    EditorGUILayout.LabelField(new GUIContent("<b>This effect is <color=#FF4040ff>very</color> resource-heavy!</b>", "Such effect significantly increases resources usage on each text refresh. Use only if really necessary."), EditorUIUtils.richMiniLabel);
                    EditorUIUtils.UnIndent();
				}

				EditorUIUtils.Header("Service Commands");

				using (EditorUIUtils.Horizontal())
				{
					groupAnchor = (LabelAnchor)EditorGUILayout.EnumPopup(
						new GUIContent("Move All To", "Use to explicitly move all counters to the specified anchor label.\n" +
					                                  "Select anchor and press Apply."), groupAnchor);

					if (GUILayout.Button(new GUIContent("Apply", "Press to move all counters to the selected anchor label."),
					                     GUILayout.Width(45)))
					{
						Undo.RegisterCompleteObjectUndo(target, "Move all counters to anchor");

						me.fpsCounter.Anchor = groupAnchor;
						fpsAnchor.enumValueIndex = (int)groupAnchor;

						me.memoryCounter.Anchor = groupAnchor;
						memoryAnchor.enumValueIndex = (int)groupAnchor;

						me.deviceInfoCounter.Anchor = groupAnchor;
						deviceAnchor.enumValueIndex = (int)groupAnchor;
						
					}
				}
				EditorUIUtils.UnIndent();
			}

			if (EditorUIUtils.Foldout(advancedFoldout, "Advanced Settings"))
			{
				EditorUIUtils.Indent();
				EditorUIUtils.DrawProperty(sortingOrder, () => me.SortingOrder = sortingOrder.intValue);
				EditorUIUtils.UnIndent();
			}

			GUI.enabled = EditorUIUtils.ToggleFoldout(fpsEnabled, fps, "FPS Counter");
			me.fpsCounter.Enabled = fpsEnabled.boolValue;

			if (fps.isExpanded)
			{
				EditorUIUtils.Indent();
				EditorUIUtils.DrawProperty(fpsInterval, "Interval", () => me.fpsCounter.UpdateInterval = fpsInterval.floatValue);
				EditorUIUtils.DrawProperty(fpsAnchor, () => me.fpsCounter.Anchor = (LabelAnchor)fpsAnchor.enumValueIndex);
                EditorUIUtils.Separator(5);

                float minVal = fpsCriticalLevelValue.intValue;
				float maxVal = fpsWarningLevelValue.intValue;

				EditorGUILayout.MinMaxSlider(new GUIContent("Colors Range", 
					"This range will be used to apply colors below on specific FPS:\n" +
					"Critical: 0 - min\n" +
					"Warning: min+1 - max-1\n" +
					"Normal: max+"), 
					ref minVal, ref maxVal, 1, 60);

				fpsCriticalLevelValue.intValue = (int)minVal;
				fpsWarningLevelValue.intValue = (int)maxVal;

				using (EditorUIUtils.Horizontal())
				{
					EditorUIUtils.DrawProperty(fpsColor, "Normal", () => me.fpsCounter.Color = fpsColor.colorValue);
					GUILayout.Label(maxVal + "+ FPS", GUILayout.Width(75));
				}

				using (EditorUIUtils.Horizontal())
				{
					EditorUIUtils.DrawProperty(fpsColorWarning, "Warning", () => me.fpsCounter.ColorWarning = fpsColorWarning.colorValue);
					GUILayout.Label((minVal + 1) + " - " + (maxVal - 1) + " FPS", GUILayout.Width(75));
				}

				using (EditorUIUtils.Horizontal())
				{
					EditorUIUtils.DrawProperty(fpsColorCritical, "Critical", () => me.fpsCounter.ColorCritical = fpsColorCritical.colorValue);
					GUILayout.Label("0 - " + minVal + " FPS", GUILayout.Width(75));
				}

				EditorUIUtils.Separator(5);

                EditorUIUtils.DrawProperty(fpsStyle, () => me.fpsCounter.Style = (FontStyle)fpsStyle.enumValueIndex);

                EditorUIUtils.Separator(5);
				EditorUIUtils.DrawProperty(fpsMilliseconds, () => me.fpsCounter.Milliseconds = fpsMilliseconds.boolValue);
				EditorUIUtils.DrawProperty(fpsAverage, "Average FPS", () => me.fpsCounter.Average = fpsAverage.boolValue);

				if (fpsAverage.boolValue)
				{
					EditorUIUtils.Indent();
					EditorUIUtils.DrawProperty(fpsAverageSamples, "Samples", () => me.fpsCounter.AverageSamples = fpsAverageSamples.intValue);
					EditorUIUtils.DrawProperty(fpsAverageMilliseconds, "Milliseconds", () => me.fpsCounter.AverageMilliseconds = fpsAverageMilliseconds.boolValue);
					EditorUIUtils.DrawProperty(fpsAverageNewLine, "New Line", () => me.fpsCounter.AverageNewLine = fpsAverageNewLine.boolValue);
					using (EditorUIUtils.Horizontal())
					{
						EditorGUILayout.PropertyField(fpsResetAverageOnNewScene, new GUIContent("Auto Reset"));
						if (GUILayout.Button("Reset", GUILayout.MaxWidth(200), GUILayout.MinWidth(40)))
						{
							me.fpsCounter.ResetAverage();
						}
					}

					EditorUIUtils.UnIndent();
				}

				EditorUIUtils.DrawProperty(fpsMinMax, "MinMax FPS", () => me.fpsCounter.MinMax = fpsMinMax.boolValue);

				if (fpsMinMax.boolValue)
				{
					EditorUIUtils.Indent();
					EditorGUILayout.PropertyField(fpsMinMaxIntervalsToSkip, new GUIContent("Delay"));
					EditorUIUtils.DrawProperty(fpsMinMaxMilliseconds, "Milliseconds", () => me.fpsCounter.MinMaxMilliseconds = fpsMinMaxMilliseconds.boolValue);
					EditorUIUtils.DrawProperty(fpsMinMaxNewLine, "New Line", () => me.fpsCounter.MinMaxNewLine = fpsMinMaxNewLine.boolValue);
					EditorUIUtils.DrawProperty(fpsMinMaxTwoLines, "Two Lines", () => me.fpsCounter.MinMaxTwoLines = fpsMinMaxTwoLines.boolValue);
					using (EditorUIUtils.Horizontal())
					{
						EditorGUILayout.PropertyField(fpsResetMinMaxOnNewScene, new GUIContent("Auto Reset"));
						if (GUILayout.Button("Reset", GUILayout.MaxWidth(200), GUILayout.MinWidth(40)))
						{
							me.fpsCounter.ResetMinMax();
						}
					}
					EditorUIUtils.UnIndent();
				}

				EditorUIUtils.DrawProperty(fpsRender, "Render Time", () => me.fpsCounter.Render = fpsRender.boolValue);
				if (fpsRender.boolValue)
				{
					EditorUIUtils.Indent();
					EditorUIUtils.DrawProperty(fpsColorRender, "Color", () => me.fpsCounter.ColorRender = fpsColorRender.colorValue);
					EditorUIUtils.DrawProperty(fpsRenderNewLine, "New Line", () => me.fpsCounter.RenderNewLine = fpsRenderNewLine.boolValue);
					EditorUIUtils.DrawProperty(fpsRenderAutoAdd, "Auto add", () => me.fpsCounter.RenderAutoAdd = fpsRenderAutoAdd.boolValue);
					EditorUIUtils.UnIndent();
				}

				EditorUIUtils.UnIndent();
			}
			GUI.enabled = true;

			GUI.enabled = EditorUIUtils.ToggleFoldout(memoryEnabled, memory, "Memory Counter");
			me.memoryCounter.Enabled = memoryEnabled.boolValue;
			if (memory.isExpanded)
			{
				EditorUIUtils.Indent();
				EditorUIUtils.DrawProperty(memoryInterval, "Interval", () => me.memoryCounter.UpdateInterval = memoryInterval.floatValue);
				EditorUIUtils.DrawProperty(memoryAnchor, () => me.memoryCounter.Anchor = (LabelAnchor)memoryAnchor.enumValueIndex);
				EditorUIUtils.DrawProperty(memoryColor, () => me.memoryCounter.Color = memoryColor.colorValue);
                EditorUIUtils.DrawProperty(memoryStyle, () => me.memoryCounter.Style = (FontStyle)memoryStyle.enumValueIndex);
                EditorGUILayout.Space();
				EditorUIUtils.DrawProperty(memoryPrecise, () => me.memoryCounter.Precise = memoryPrecise.boolValue);
				EditorUIUtils.DrawProperty(memoryTotal, () => me.memoryCounter.Total = memoryTotal.boolValue);
				EditorUIUtils.DrawProperty(memoryAllocated, () => me.memoryCounter.Allocated = memoryAllocated.boolValue);
				EditorUIUtils.DrawProperty(memoryMonoUsage, "Mono", () => me.memoryCounter.MonoUsage = memoryMonoUsage.boolValue);
				EditorUIUtils.UnIndent();
            }
			GUI.enabled = true;

			GUI.enabled = EditorUIUtils.ToggleFoldout(deviceEnabled, device, "Device Information");
			me.deviceInfoCounter.Enabled = deviceEnabled.boolValue;
			if (device.isExpanded)
			{
				EditorUIUtils.Indent();
				EditorUIUtils.DrawProperty(deviceAnchor, () => me.deviceInfoCounter.Anchor = (LabelAnchor)deviceAnchor.intValue);
				EditorUIUtils.DrawProperty(deviceColor, () => me.deviceInfoCounter.Color = deviceColor.colorValue);
                EditorUIUtils.DrawProperty(deviceStyle, () => me.deviceInfoCounter.Style = (FontStyle)deviceStyle.enumValueIndex);
                EditorGUILayout.Space();
				EditorUIUtils.DrawProperty(devicePlatform, "Platform", () => me.deviceInfoCounter.Platform = devicePlatform.boolValue);
				EditorUIUtils.DrawProperty(deviceCpuModel, "CPU", () => me.deviceInfoCounter.CpuModel = deviceCpuModel.boolValue);
				EditorUIUtils.DrawProperty(deviceGpuModel, "GPU Model", () => me.deviceInfoCounter.GpuModel = deviceGpuModel.boolValue);
				EditorUIUtils.DrawProperty(deviceGpuApi, "GPU API", () => me.deviceInfoCounter.GpuApi = deviceGpuApi.boolValue);
				EditorUIUtils.DrawProperty(deviceGpuSpec, "GPU Spec", () => me.deviceInfoCounter.GpuSpec = deviceGpuSpec.boolValue);
				EditorUIUtils.DrawProperty(deviceRamSize, "RAM", () => me.deviceInfoCounter.RamSize = deviceRamSize.boolValue);
				EditorUIUtils.DrawProperty(deviceScreenData, "Screen", () => me.deviceInfoCounter.ScreenData = deviceScreenData.boolValue);
				EditorUIUtils.DrawProperty(deviceModel, "Model", () => me.deviceInfoCounter.DeviceModel = deviceModel.boolValue);
				EditorUIUtils.UnIndent();
			}
			GUI.enabled = true;
			EditorGUILayout.Space();
			serializedObject.ApplyModifiedProperties();
		}

		private void AddRenderRecorderToCamera()
		{
			Camera mainCamera = Camera.main;
			if (mainCamera == null)
			{
				EditorUtility.DisplayDialog("Advanced FPS Counter", "Can't find main camera in scene to add AFPSRenderRecorder there!\n" +
					"Please consider adding it manually or set MainCamera tag to pne of your cameras and try again.", "Oh, fine");
				return;
			}

			if (mainCamera.gameObject.GetComponent<AFPSRenderRecorder>() != null)
			{
				if (!EditorUtility.DisplayDialog("Advanced FPS Counter", "Your main camera already has AFPSRenderRecorder added!", "Ah, cool", "Show me that camera!"))
				{
					Selection.activeGameObject = mainCamera.gameObject;
				}
				return;
			}

			AFPSRenderRecorder recorder = mainCamera.gameObject.AddComponent<AFPSRenderRecorder>();
			EditorUtility.SetDirty(recorder);

			if (!EditorUtility.DisplayDialog("Advanced FPS Counter", "AFPSRenderRecorder successfully added to the camera with MainCamera tag!", "Thanks!", "Thanks, show me that camera"))
			{
				Selection.activeGameObject = mainCamera.gameObject;
			}
		}
	}
}
#endif