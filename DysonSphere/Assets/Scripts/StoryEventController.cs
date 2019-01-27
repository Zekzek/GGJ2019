using UnityEngine;

public class StoryEventController : MonoBehaviour
{
	[SerializeField]
	private StoryEventScenarioUI scenarioUIPrefab;

	private StoryEventScenarioUI scenarioUIInstance;

	[SerializeField]
	private StoryEventResultUI resultUIPrefab;

	private StoryEventResultUI resultUIInstance;

	public void ShowScenario(ChoiceEvent choiceEvent)
	{
		Time.timeScale = 0;

		scenarioUIInstance = Instantiate(scenarioUIPrefab);
		scenarioUIInstance.Setup(choiceEvent, this);
	}

	public void PickOption(ChoiceEvent.Option option)
	{
		Destroy(scenarioUIInstance.gameObject);

		string result = option.OnChoosen();

		resultUIInstance = Instantiate(resultUIPrefab);
		resultUIInstance.Setup(result, this);
	}

	public void CloseResults()
	{
		Destroy(resultUIInstance.gameObject);

		Time.timeScale = 1;
	}
}
