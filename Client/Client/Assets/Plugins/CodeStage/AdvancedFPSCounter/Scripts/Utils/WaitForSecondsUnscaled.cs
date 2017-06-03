#if UNITY_5_3_OR_NEWER
using UnityEngine;

namespace CodeStage.AdvancedFPSCounter
{
	public class WaitForSecondsUnscaled : CustomYieldInstruction
	{
		private readonly float waitTime;
		private float runUntil;

		public override bool keepWaiting
		{
			get { return Time.unscaledTime < runUntil; }
		}

		/// <summary>
		/// Resets YieldInstruction for further re-use. Call it just before re-using.
		/// </summary>
		public new void Reset()
		{
			runUntil = Time.unscaledTime + waitTime;
		}

		/// <summary>
		/// Creates a yield instruction to wait for a given number of seconds using unscaled time.
		/// </summary>
		/// <param name="time">Time to wait before proceeding.</param>
		public WaitForSecondsUnscaled(float time)
		{
			waitTime = time;
		}
	}
}
#endif