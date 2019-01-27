﻿using UnityEngine;
using System.Collections;
using TMPro;

public class StoryEventScenarioUI : Popup
{
	[SerializeField]
	private TextMeshProUGUI titleText;

	[SerializeField]
	private StoryEventOptionUI optionPrefab;

	[SerializeField]
	private TextMeshProUGUI scenarioText;

	[SerializeField]
	private Transform optionList;

	public void Setup(ChoiceEvent choiceEvent)
	{
        titleText.SetText(choiceEvent.title);
		scenarioText.SetText(choiceEvent.scenario);
		foreach (ChoiceEvent.Option option in choiceEvent.options)
		{
			StoryEventOptionUI optionUI = Instantiate(optionPrefab, optionList);
			optionUI.Setup(option);
		}
	}
}
