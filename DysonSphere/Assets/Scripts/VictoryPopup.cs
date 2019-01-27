using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VictoryPopup : Popup
{
	[SerializeField]
	private Button button;

	public void Awake()
	{
		button.onClick.AddListener(() => { SceneManager.LoadScene(0); });
	}
}
