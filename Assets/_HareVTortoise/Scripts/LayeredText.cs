using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LayeredText : MonoBehaviour
{

	#region Member Declarations

	[SerializeProperty("Text")]
	public string _text = "";

	public string Text {
		get => _text;
		set { _text = value; UpdateText(); }
	}

	[SerializeProperty("FontSize")]
	public float _fontSize = 16.0f;

	public float FontSize
	{
		get => _fontSize;
		set { _fontSize = value; UpdateText(); }
	}

	[SerializeField]
	private List<TextMeshProUGUI> textMeshProUGUIs;

	#endregion

	#region Monobehaviour

	private void OnEnable()
	{
		UpdateText();
	}

	#endregion

	#region Helper Functions

	private void UpdateText()
	{
		foreach (TextMeshProUGUI tmp in textMeshProUGUIs)
		{
			tmp.text = Text;
			tmp.fontSize = FontSize;
		}
	}

	#endregion
}
