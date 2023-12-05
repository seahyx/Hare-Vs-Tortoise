using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameLevelManager))]
public class WinLosePanelManager : MonoBehaviour
{
	#region Serializables

	[Header("Win Panel")]

	[SerializeField]
	private GameObject winPanel;

	[SerializeField]
	private LayeredText livesText;

	[Header("Lose Panel")]

	[SerializeField]
	private GameObject losePanel;

	#endregion

	#region Member Declarations

	private GameLevelManager gameLevelManager;

	#endregion

	#region Monobehaviour

	private void Awake()
	{
		gameLevelManager = GetComponent<GameLevelManager>();
		gameLevelManager.OnWin.AddListener(ShowWinPanel);
		gameLevelManager.OnLose.AddListener(ShowlosePanel);
	}

	#endregion

	#region Helper Functions

	public void ShowWinPanel(GameLevelManager.WinData winData)
	{
		losePanel?.SetActive(false);
		winPanel?.SetActive(true);

		if (livesText != null)
			livesText.Text = winData.lives.ToString();
	}

	public void ShowlosePanel()
	{
		winPanel?.SetActive(false);
		losePanel?.SetActive(true);
	}

	#endregion
}
