using UnityEngine;
using System.Collections;
using TMPro;

public class StoryEventScenarioUI : MonoBehaviour
{
	[SerializeField]
	private StoryEventOptionUI optionPrefab;

	[SerializeField]
	private TextMeshProUGUI scenarioText;

	[SerializeField]
	private Transform optionList;

	public void Setup(ChoiceEvent choiceEvent)
	{
		foreach (ChoiceEvent.Option option in choiceEvent.options)
		{
			StoryEventOptionUI optionUI = Instantiate(optionPrefab, optionList);
			optionUI.Setup(option);
		}
	}
}
