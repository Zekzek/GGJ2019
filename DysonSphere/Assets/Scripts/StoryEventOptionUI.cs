using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System;

public class StoryEventOptionUI : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI optionText;

	[SerializeField]
	private Button optionButton;

	private ChoiceEvent.Option option;

	private StoryEventController storyEventController;

	public void Setup(ChoiceEvent.Option option, StoryEventController storyEventController)
	{
		this.option = option;
		optionText.SetText(option.option);
		optionButton.onClick.AddListener(ClickListener);
		this.storyEventController = storyEventController;
	}

	private void ClickListener()
	{
		storyEventController.PickOption(option);
	}
}
