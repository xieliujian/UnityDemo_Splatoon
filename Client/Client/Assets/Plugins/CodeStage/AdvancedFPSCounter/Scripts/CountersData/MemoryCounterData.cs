using System.Collections;
using UnityEngine;

#if UNITY_5_5_OR_NEWER
using UnityEngine.Profiling;
#endif


namespace CodeStage.AdvancedFPSCounter.CountersData
{
	/// <summary>
	/// Shows memory usage data.
	/// </summary>
	[AddComponentMenu("")]
	[System.Serializable]
	public class MemoryCounterData: UpdatebleCounterData
	{
		// ----------------------------------------------------------------------------
		// constants
		// ----------------------------------------------------------------------------

		public const long MEMORY_DIVIDER = 1048576; // 1024^2

		private const string TEXT_START = "<color=#{0}>";
		private const string LINE_START_TOTAL = "MEM TOTAL: ";
		private const string LINE_START_ALLOCATED = "MEM ALLOC: ";
		private const string LINE_START_MONO = "MEM MONO: ";
		private const string LINE_END = " MB";
		private const string TEXT_END = "</color>";

		// ----------------------------------------------------------------------------
		// properties exposed to the inspector
		// ----------------------------------------------------------------------------

		#region Precise
		[Tooltip("Allows to output memory usage more precisely thus using a bit more system resources.")]
		[SerializeField]
		private bool precise = true;

		/// <summary>
		/// Allows to output memory usage more precisely thus using a bit more system resources.
		/// </summary>
		public bool Precise
		{
			get { return precise; }
			set
			{
				if (precise == value || !Application.isPlaying) return;
				precise = value;
				if (!enabled) return;

				Refresh();
			}
		}
		#endregion

		#region Total
		[Tooltip("Allows to see private memory amount reserved for application. This memory can’t be used by other applications.")]
		[SerializeField]
		private bool total = true;

		/// <summary>
		/// Allows to see private memory amount reserved for application. This memory can’t be used by other applications.
		/// </summary>
		public bool Total
		{
			get { return total; }
			set
			{
				if (total == value || !Application.isPlaying) return;
				total = value;
				if (!total) LastTotalValue = 0;
				if (!enabled) return;

				Refresh();
			}
		}
		#endregion

		#region Allocated
		[Tooltip("Allows to see amount of memory, currently allocated by application.")]
		[SerializeField]
		private bool allocated = true;

		/// <summary>
		/// Allows to see amount of memory, currently allocated by application.
		/// </summary>
		public bool Allocated
		{
			get { return allocated; }
			set
			{
				if (allocated == value || !Application.isPlaying) return;
				allocated = value;
				if (!allocated) LastAllocatedValue = 0;
				if (!enabled) return;

				Refresh();
			}
		}
		#endregion

		#region MonoUsage
		[Tooltip("Allows to see amount of memory, allocated by managed Mono objects, " +
		         "such as UnityEngine.Object and everything derived from it for example.")]
		[SerializeField]
		private bool monoUsage;

		/// <summary>
		/// Allows to see amount of memory, allocated by managed Mono objects, 
		/// such as UnityEngine.Object and everything derived from it for example.
		/// </summary>
		public bool MonoUsage
		{
			get { return monoUsage; }
			set
			{
				if (monoUsage == value || !Application.isPlaying) return;
				monoUsage = value;
				if (!monoUsage) LastMonoValue = 0;
				if (!enabled) return;

				Refresh();
			}
		}
		#endregion

		// ----------------------------------------------------------------------------
		// properties only accessible from code
		// ----------------------------------------------------------------------------

		/// <summary>
		/// Last total memory readout.
		/// </summary>
		/// In megabytes if #Precise is false, in bytes otherwise.
		/// @see Total
		public long LastTotalValue { get; private set; }

		/// <summary>
		/// Last allocated memory readout.
		/// </summary>
		/// In megabytes if #Precise is false, in bytes otherwise.
		/// @see Allocated
		public long LastAllocatedValue { get; private set; }

		/// <summary>
		/// Last Mono memory readout.
		/// </summary>
		/// In megabytes if #Precise is false, in bytes otherwise.
		/// @see MonoUsage
		public long LastMonoValue { get; private set; }

		// ----------------------------------------------------------------------------
		// constructor
		// ----------------------------------------------------------------------------

		internal MemoryCounterData()
		{
			color = new Color32(234, 238, 101, 255);
			style = FontStyle.Bold;
		}

		// ----------------------------------------------------------------------------
		// internal methods
		// ----------------------------------------------------------------------------

		internal override void UpdateValue(bool force)
		{
			if (!enabled) return;

			if (force)
			{
				if (!inited && (HasData()))
				{
					Activate();
					return;
				}

				if (inited && (!HasData()))
				{
					Deactivate();
					return;
				}
			}

			if (total)
			{
#if UNITY_5_6_OR_NEWER
				long value = Profiler.GetTotalReservedMemoryLong();
#else
				long value = Profiler.GetTotalReservedMemory();
#endif
				long divisionResult = 0;

				bool newValue;
				if (precise)
				{
					newValue = (LastTotalValue != value);
				}
				else
				{
					divisionResult = value / MEMORY_DIVIDER;
					newValue = (LastTotalValue != divisionResult);
				}

				if (newValue || force)
				{
					LastTotalValue = precise ? value : divisionResult;
					dirty = true;
				}
			}

			if (allocated)
			{
#if UNITY_5_6_OR_NEWER
				long value = Profiler.GetTotalAllocatedMemoryLong();
#else
				long value = Profiler.GetTotalAllocatedMemory();
#endif
				long divisionResult = 0;

				bool newValue;
				if (precise)
				{
					newValue = (LastAllocatedValue != value);
				}
				else
				{
					divisionResult = value / MEMORY_DIVIDER;
					newValue = (LastAllocatedValue != divisionResult);
				}

				if (newValue || force)
				{
					LastAllocatedValue = precise ? value : divisionResult;
					dirty = true;
				}
			}

			if (monoUsage)
			{
				long monoMemory = System.GC.GetTotalMemory(false);
				long monoDivisionResult = 0;

				bool newValue;
				if (precise)
				{
					newValue = (LastMonoValue != monoMemory);
				}
				else
				{
					monoDivisionResult = monoMemory / MEMORY_DIVIDER;
					newValue = (LastMonoValue != monoDivisionResult);
				}

				if (newValue || force)
				{
					LastMonoValue = precise ? monoMemory : monoDivisionResult;
					dirty = true;
				}
			}

			if (!dirty || main.OperationMode != OperationMode.Normal) return;

			bool needNewLine = false;

			text.Length = 0;
			text.Append(colorCached);

			if (total)
			{
				text.Append(LINE_START_TOTAL);

				if (precise)
				{
					text.Append((LastTotalValue / (float)MEMORY_DIVIDER).ToString("F"));
				}
				else
				{
					text.Append(LastTotalValue);
				}
				text.Append(LINE_END);
				needNewLine = true;
			}

			if (allocated)
			{
				if (needNewLine) text.Append(AFPSCounter.NEW_LINE);
				text.Append(LINE_START_ALLOCATED);

				if (precise)
				{
					text.Append((LastAllocatedValue / (float)MEMORY_DIVIDER).ToString("F"));
				}
				else
				{
					text.Append(LastAllocatedValue);
				}
				text.Append(LINE_END);
				needNewLine = true;
			}

			if (monoUsage)
			{
				if (needNewLine) text.Append(AFPSCounter.NEW_LINE);
				text.Append(LINE_START_MONO);

				if (precise)
				{
					text.Append((LastMonoValue / (float)MEMORY_DIVIDER).ToString("F"));
				}
				else
				{
					text.Append(LastMonoValue);
				}

				text.Append(LINE_END);
			}

			text.Append(TEXT_END);

			ApplyTextStyles();
		}

		// ----------------------------------------------------------------------------
		// protected methods
		// ----------------------------------------------------------------------------

		protected override void PerformActivationActions()
		{
			base.PerformActivationActions();

			if (!HasData()) return;

			LastTotalValue = 0;
			LastAllocatedValue = 0;
			LastMonoValue = 0;

			if (main.OperationMode == OperationMode.Normal)
			{
				if (colorCached == null)
				{
					colorCached = string.Format(TEXT_START, AFPSCounter.Color32ToHex(color));
				}

				text.Append(colorCached);

				if (total)
				{
					if (precise)
					{
						text.Append(LINE_START_TOTAL).Append("0.00").Append(LINE_END);
					}
					else
					{
						text.Append(LINE_START_TOTAL).Append(0).Append(LINE_END);
					}
				}

				if (allocated)
				{
					if (text.Length > 0) text.Append(AFPSCounter.NEW_LINE);
					if (precise)
					{
						text.Append(LINE_START_ALLOCATED).Append("0.00").Append(LINE_END);
					}
					else
					{
						text.Append(LINE_START_ALLOCATED).Append(0).Append(LINE_END);
					}
				}

				if (monoUsage)
				{
					if (text.Length > 0) text.Append(AFPSCounter.NEW_LINE);
					if (precise)
					{
						text.Append(LINE_START_MONO).Append("0.00").Append(LINE_END);
					}
					else
					{
						text.Append(LINE_START_MONO).Append(0).Append(LINE_END);
					}
				}

				text.Append(TEXT_END);

				ApplyTextStyles();

				dirty = true;
			}
		}

		protected override void PerformDeActivationActions()
		{
			base.PerformDeActivationActions();

			if (text != null) text.Length = 0;

			main.MakeDrawableLabelDirty(anchor);
		}

		protected override IEnumerator UpdateCounter()
		{
			while (true)
			{
				UpdateValue();
				main.UpdateTexts();
#if UNITY_5_3_OR_NEWER
				cachedWaitForSecondsUnscaled.Reset();
				yield return cachedWaitForSecondsUnscaled;
#else
				yield return cachedWaitForSeconds;
#endif
			}
		}

		protected override bool HasData()
		{
			return total || allocated || monoUsage;
		}

		protected override void CacheCurrentColor()
		{
			colorCached = string.Format(TEXT_START, AFPSCounter.Color32ToHex(color));
		}
	}
}