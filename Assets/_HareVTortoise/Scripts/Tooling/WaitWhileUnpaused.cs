using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitWhileUnpaused : CustomYieldInstruction
{
	private float waitDuration = 0.0f;
	private IPausable pauseable;

	public WaitWhileUnpaused(float duration, IPausable pauseable)
	{
		waitDuration = duration;
		this.pauseable = pauseable;
	}

	public override bool keepWaiting
	{
		get
		{
			if (!pauseable.IsPaused)
			{
				waitDuration -= Time.deltaTime;
			}
			return waitDuration >= 0.0f;
		}
	}
}
