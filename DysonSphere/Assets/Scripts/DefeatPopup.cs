using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DefeatPopup : Popup
{
	[SerializeField]
	private Button button;

	[SerializeField]
	private TextMeshProUGUI resultText;

	public void Awake()
	{
		resultText.SetText(GameState.Instance.GenerateSynopsis());
		button.onClick.AddListener(() => { Time.timeScale = 1; SceneManager.LoadScene(0); });
	}
}
