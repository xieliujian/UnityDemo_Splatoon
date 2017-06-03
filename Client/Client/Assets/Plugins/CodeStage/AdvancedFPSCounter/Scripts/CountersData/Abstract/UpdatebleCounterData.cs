using System.Collections;
using UnityEngine;

namespace CodeStage.AdvancedFPSCounter.CountersData
{
	public abstract class UpdatebleCounterData : BaseCounterData
	{
		// ----------------------------------------------------------------------------
		// protected fields
		// ----------------------------------------------------------------------------

		protected Coroutine updateCoroutine;

#if UNITY_5_3_OR_NEWER
		protected WaitForSecondsUnscaled cachedWaitForSecondsUnscaled;
#else
		protected WaitForSeconds cachedWaitForSeconds;
#endif

		// ----------------------------------------------------------------------------
		// properties exposed to the inspector
		// ----------------------------------------------------------------------------

		#region UpdateInterval
		[Tooltip("Update interval in seconds.")]
		[Range(0.1f, 10f)]
		[SerializeField]
		protected float updateInterval = 0.5f;

		/// <summary>
		/// Update interval in seconds.
		/// </summary>
		public float UpdateInterval
		{
			get { return updateInterval; }
			set
			{
				if (System.Math.Abs(updateInterval - value) < 0.001f || !Application.isPlaying) return;

				updateInterval = value;
				CacheWaitForSeconds();
			}
		}
		#endregion

		// ----------------------------------------------------------------------------
		// protected methods
		// ----------------------------------------------------------------------------

		protected override void PerformInitActions()
		{
			base.PerformInitActions();

			StartUpdateCoroutine();
		}

		protected override void PerformActivationActions()
		{
			base.PerformActivationActions();

			CacheWaitForSeconds();
		}

		protected override void PerformDeActivationActions()
		{
			base.PerformDeActivationActions();

			StoptUpdateCoroutine();
		}

		protected abstract IEnumerator UpdateCounter();

		// ----------------------------------------------------------------------------
		// private methods
		// ----------------------------------------------------------------------------

		private void StartUpdateCoroutine()
		{
			updateCoroutine = main.StartCoroutine(UpdateCounter());
		}

		private void StoptUpdateCoroutine()
		{
			main.StopCoroutine(updateCoroutine);
		}

		private void CacheWaitForSeconds()
		{
#if UNITY_5_3_OR_NEWER
			cachedWaitForSecondsUnscaled = new WaitForSecondsUnscaled(updateInterval);
#else
			cachedWaitForSeconds = new WaitForSeconds(updateInterval);
#endif
		}
	}
}