using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave Blueprint", menuName = "Wave System/Wave Blueprint", order = 1)]
public class WaveBPScriptableObject : ScriptableObject
{
	#region Serializables

	[SerializeField, Tooltip("Description of this wave blueprint."), TextArea(2, 4)]
    public string Description = "";

    [SerializeField, Tooltip("Wave sequence.")]
    public List<WaveBPItem> WaveSequence = new List<WaveBPItem>();

	#endregion

	public IEnumerator Execute(WaveManager waveManager)
	{
		int currentWaveItemIdx = 0;
		while (currentWaveItemIdx < WaveSequence.Count)
		{
			yield return waveManager.StartCoroutine(WaveSequence[currentWaveItemIdx].Execute(waveManager));
			currentWaveItemIdx++;
		}
	}
}
