using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class StoryEventResultUI : Popup
{
	[SerializeField]
	private TextMeshProUGUI resultText;

	[SerializeField]
	private Button okButton;

	public void Setup(string result)
	{
		resultText.SetText(result);
		okButton.onClick.AddListener(OnClick);
	}

	public void OnClick()
	{
		StoryEventController.Instance.CloseResults();
	}
}
