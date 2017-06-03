#define UNITY_5_3_2_PLUS
#if UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3_0 || UNITY_5_3_1
#undef UNITY_5_3_2_PLUS
#endif

using System.Text;
using CodeStage.AdvancedFPSCounter.Labels;
using UnityEngine;

namespace CodeStage.AdvancedFPSCounter.CountersData
{
	/// <summary>
	/// Shows additional device information.
	/// </summary>
	[AddComponentMenu("")]
	[System.Serializable]
	public class DeviceInfoCounterData: BaseCounterData
	{
		// ----------------------------------------------------------------------------
		// properties exposed to the inspector
		// ----------------------------------------------------------------------------

		#region Platform
		[Tooltip("Shows operating system & platform info.")]
		[SerializeField]
		private bool platform = true;

		/// <summary>
		/// Shows operating system & platform info.
		/// </summary>
		public bool Platform
		{
			get { return platform; }
			set
			{
				if (platform == value || !Application.isPlaying) return;
				platform = value;
				if (!enabled) return;

				Refresh();
			}
		}
		#endregion

		#region CpuModel
		[Tooltip("CPU model and cores (including virtual cores from Intel's Hyper Threading) count.")]
		[SerializeField]
		private bool cpuModel = true;

		/// <summary>
		/// Shows CPU model and cores (including virtual cores from Intel's Hyper Threading) count.
		/// </summary>
		public bool CpuModel
		{
			get { return cpuModel; }
			set
			{
				if (cpuModel == value || !Application.isPlaying) return;
				cpuModel = value;
				if (!enabled) return;

				Refresh();
			}
		}
		#endregion

		#region GpuModel
		[Tooltip("Shows GPU model name.")]
		[SerializeField]
		private bool gpuModel = true;

		/// <summary>
		/// Shows GPU model.
		/// </summary>
		public bool GpuModel
		{
			get { return gpuModel; }
			set
			{
				if (gpuModel == value || !Application.isPlaying) return;
				gpuModel = value;
				if (!enabled) return;

				Refresh();
			}
		}
		#endregion

		#region GpuApi
		[Tooltip("Shows graphics API version and type (if possible).")]
		[SerializeField]
		private bool gpuApi = true;

		/// <summary>
		/// Shows graphics API version and type (if possible).
		/// </summary>
		public bool GpuApi
		{
			get { return gpuApi; }
			set
			{
				if (gpuApi == value || !Application.isPlaying) return;
				gpuApi = value;
				if (!enabled) return;

				Refresh();
			}
		}
		#endregion

		#region GpuSpec
		[Tooltip("Shows graphics supported shader model (if possible), approximate pixel fill-rate (if possible) and total Video RAM size (if possible).")]
		[SerializeField]
		private bool gpuSpec = true;

		/// <summary>
		/// Shows graphics supported shader model (if possible), approximate pixel fill-rate (if possible) and total Video RAM size (if possible).
		/// </summary>
		public bool GpuSpec
		{
			get { return gpuSpec; }
			set
			{
				if (gpuSpec == value || !Application.isPlaying) return;
				gpuSpec = value;
				if (!enabled) return;

				Refresh();
			}
		}
		#endregion

		#region RAMSize
		[Tooltip("Shows total RAM size.")]
		[SerializeField]
		private bool ramSize = true;

		/// <summary>
		/// Shows total RAM size.
		/// </summary>
		public bool RamSize
		{
			get { return ramSize; }
			set
			{
				if (ramSize == value || !Application.isPlaying) return;
				ramSize = value;
				if (!enabled) return;

				Refresh();
			}
		}
		#endregion

		#region ScreenData
		[Tooltip("Shows screen resolution, size and DPI (if possible).")]
		[SerializeField]
		private bool screenData = true;

		/// <summary>
		/// Shows screen resolution, size and DPI (if possible).
		/// </summary>
		public bool ScreenData
		{
			get { return screenData; }
			set
			{
				if (screenData == value || !Application.isPlaying) return;
				screenData = value;
				if (!enabled) return;

				Refresh();
			}
		}
        #endregion

        #region DeviceModel
        [Tooltip("Shows device model. Actual for mobile devices.")]
        [SerializeField]
        private bool deviceModel;

        /// <summary>
        /// Shows device model.
        /// </summary>
        public bool DeviceModel
        {
            get { return deviceModel; }
            set
            {
                if (deviceModel == value || !Application.isPlaying) return;
                deviceModel = value;
                if (!enabled) return;

                Refresh();
            }
        }
        #endregion

        // ----------------------------------------------------------------------------
        // properties only accessible from code
        // ----------------------------------------------------------------------------

        public string LastValue { get; private set; }

		// ----------------------------------------------------------------------------
		// constructor
		// ----------------------------------------------------------------------------

		internal DeviceInfoCounterData()
		{
			color = new Color32(172, 172, 172, 255);
			anchor = LabelAnchor.LowerLeft;
		}

		// ----------------------------------------------------------------------------
		// internal methods
		// ----------------------------------------------------------------------------

		internal override void UpdateValue(bool force)
		{
			if (!inited && HasData())
			{
				Activate();
				return;
			}

			if (inited && !HasData())
			{
				Deactivate();
				return;
			}

			if (!enabled) return;

			bool needNewLine = false;

			if (text == null)
			{
				text = new StringBuilder(500);
			}
			else
			{
				text.Length = 0;
			}

			if (platform)
			{
				text.Append("OS: ").Append(SystemInfo.operatingSystem)
					.Append(" [").Append(Application.platform).Append("]");
				needNewLine = true;
			}

			if (cpuModel)
			{
				if (needNewLine) text.Append(AFPSCounter.NEW_LINE);

				text.Append("CPU: ").Append(SystemInfo.processorType).Append(" [").Append(SystemInfo.processorCount).Append(" cores]");
				needNewLine = true;
			}

			if (gpuModel)
			{
				if (needNewLine) text.Append(AFPSCounter.NEW_LINE);
				text.Append("GPU: ").Append(SystemInfo.graphicsDeviceName);
				needNewLine = true;
			}

			if (gpuApi)
			{
				if (needNewLine) text.Append(AFPSCounter.NEW_LINE);
				text.Append("GPU: ").Append(SystemInfo.graphicsDeviceVersion);
#if UNITY_5_3_2_PLUS
				text.Append(" [").Append(SystemInfo.graphicsDeviceType).Append("]");
#endif
				needNewLine = true;
			}

			if (gpuSpec)
			{
				if (needNewLine) text.Append(AFPSCounter.NEW_LINE);

				text.Append("GPU: SM: ");
				int sm = SystemInfo.graphicsShaderLevel;
				if (sm >= 10 && sm <= 99)
				{
					// getting first and second digits from sm
					text.Append(sm /= 10).Append('.').Append(sm / 10);
				}
				else
				{
					text.Append("N/A");
				}

#if !UNITY_5 && !UNITY_5_3_OR_NEWER
				text.Append(", FR: ");
				int fillRate = SystemInfo.graphicsPixelFillrate;
				if (fillRate > 0)
				{
					text.Append(fillRate).Append(" MP/S");
				}
				else
				{
					text.Append("N/A");
				}
#endif
				text.Append(", VRAM: ");
				int vram = SystemInfo.graphicsMemorySize;
				if (vram > 0)
				{
					text.Append(vram).Append(" MB");
				}
				else
				{
					text.Append("N/A");
				}
				needNewLine = true;
			}

			if (ramSize)
			{
				if (needNewLine) text.Append(AFPSCounter.NEW_LINE);

				int ram = SystemInfo.systemMemorySize;

				if (ram > 0)
				{
					text.Append("RAM: ").Append(ram).Append(" MB");
					needNewLine = true;
				}
				else
				{
					needNewLine = false;
				}
			}

			if (screenData)
			{
				if (needNewLine) text.Append(AFPSCounter.NEW_LINE);
				Resolution res = Screen.currentResolution;

				text.Append("SCR: ").Append(res.width).Append("x").Append(res.height).Append("@").Append(res.refreshRate).Append("Hz [window size: ").Append(Screen.width).Append("x").Append(Screen.height);
				float dpi = Screen.dpi;
				if (dpi > 0)
				{
					text.Append(", DPI: ").Append(dpi).Append("]");
				}
				else
				{
					text.Append("]");
				}
                needNewLine = true;
            }

		    if (deviceModel)
		    {
		        if (needNewLine) text.Append(AFPSCounter.NEW_LINE);
		        text.Append("Model: ").Append(SystemInfo.deviceModel);
		    }

		    LastValue = text.ToString();

			if (main.OperationMode == OperationMode.Normal)
			{
				text.Insert(0, colorCached);
				text.Append("</color>");

                ApplyTextStyles();
            }
			else
			{
				text.Length = 0;
			}

			dirty = true;
		}

		// ----------------------------------------------------------------------------
		// protected methods
		// ----------------------------------------------------------------------------

		protected override bool HasData()
		{
			return cpuModel || gpuModel || ramSize || screenData;
		}

		protected override void CacheCurrentColor()
		{
			colorCached = "<color=#" + AFPSCounter.Color32ToHex(color) + ">";
		}
	}
}
