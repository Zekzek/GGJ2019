using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class StoryEventResultUI : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI resultText;

	[SerializeField]
	private Button okButton;

	private StoryEventController storyEventController;

	public void Setup(string result, StoryEventController storyEventController)
	{
		resultText.SetText(result);
		okButton.onClick.AddListener(OnClick);
		this.storyEventController = storyEventController;
	}

	public void OnClick()
	{
		storyEventController.CloseResults();
	}
}
