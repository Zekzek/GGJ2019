using System.Collections;
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
					instance = Resources.Load<StoryEventController>("StoryEventController");
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

	float nextEventTime;

	private static float PickNextEventTime()
	{
		return Random.Range(60, 120) + Time.time;
	}

	private void Awake()
	{
		nextEventTime = PickNextEventTime();
        ShowScenario(new UnrestEvent());
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
		scenarioUIInstance = PopupManager.Show(scenarioUIPrefab);
		scenarioUIInstance.Setup(choiceEvent);
	}

	public void PickOption(ChoiceEvent.Option option)
	{
		scenarioUIInstance.Close();

		string result = option.OnChoosen();

		resultUIInstance = PopupManager.Show(resultUIPrefab);
		resultUIInstance.Setup(result);
	}

	public void CloseResults()
	{
		resultUIInstance.Close();
	}
}
