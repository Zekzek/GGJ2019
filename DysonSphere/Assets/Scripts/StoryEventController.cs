using UnityEngine;

public class StoryEventController : MonoBehaviour
{
	private static StoryEventController instance;
	public static StoryEventController Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<StoryEventController>();
				if (instance == null)
				{
					GameObject go = new GameObject("StoryEventController");
					instance = go.AddComponent<StoryEventController>();
				}
			}

			return instance;
		}
	}

	[SerializeField]
	private StoryEventScenarioUI scenarioUIPrefab;

	private StoryEventScenarioUI scenarioUIInstance;

	[SerializeField]
	private StoryEventResultUI resultUIPrefab;

	private StoryEventResultUI resultUIInstance;

	float nextEventTime = PickNextEventTime();

	private static float PickNextEventTime()
	{
		return Random.Range(60, 120) + Time.time;
	}

	public void Update()
	{
		if (nextEventTime < Time.time)
		{
			ShowScenario(ChoiceEvent.GenerateRandomChoiceEvent());
		}
	}

	public void ShowScenario(ChoiceEvent choiceEvent)
	{
		nextEventTime = PickNextEventTime();
		Time.timeScale = 0;

		scenarioUIInstance = Instantiate(scenarioUIPrefab);
		scenarioUIInstance.Setup(choiceEvent);
	}

	public void PickOption(ChoiceEvent.Option option)
	{
		Destroy(scenarioUIInstance.gameObject);

		string result = option.OnChoosen();

		resultUIInstance = Instantiate(resultUIPrefab);
		resultUIInstance.Setup(result);
	}

	public void CloseResults()
	{
		Destroy(resultUIInstance.gameObject);

		Time.timeScale = 1;
	}
}
