using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave Blueprint", menuName = "Hare V Tortoise/Wave Blueprint", order = 1)]
public class WaveBPScriptableObject : ScriptableObject, ISerializationCallbackReceiver
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

	public void OnAfterDeserialize() { }

	public void OnBeforeSerialize()
	{
		CountTotalWaves();
	}

	public int CountTotalWaves()
	{
		int wave = 0;

		for (int i = 0; i < WaveSequence.Count; i++)
		{
			WaveBPItem item = WaveSequence[i];
			item.currentWaveNumber = wave;
			if (item.Type == WaveBPItem.ItemType.WaveEnd) wave++;
			if (item.Type == WaveBPItem.ItemType.WaveBP && item.WaveBP != null)
			{
				wave += item.WaveBP.CountTotalWaves();
			}
		}
		return wave;
	}
}
