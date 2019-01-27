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

	public void Setup(ChoiceEvent.Option option)
	{
		this.option = option;
		optionText.SetText(option.option);
		optionButton.onClick.AddListener(ClickListener);
	}

	private void ClickListener()
	{
		StoryEventController.Instance.PickOption(option);
	}
}
