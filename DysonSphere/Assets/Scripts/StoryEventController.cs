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
		StartCoroutine(AnimateIntro(scenarioUIInstance.transform));
		scenarioUIInstance.Setup(choiceEvent);
	}

	public void PickOption(ChoiceEvent.Option option)
	{
		StartCoroutine(AnimateOutro(scenarioUIInstance.transform));

		string result = option.OnChoosen();

		resultUIInstance = Instantiate(resultUIPrefab);
		StartCoroutine(AnimateIntro(resultUIInstance.transform));
		resultUIInstance.Setup(result);
	}

	public void CloseResults()
	{
		StartCoroutine(AnimateOutro(resultUIInstance.transform));

		Time.timeScale = 1;
	}



	public static IEnumerator AnimateIntro(Transform transform)
	{
		float duration = .5f;
		for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
		{
			float v = t / duration;
			foreach (Transform child in transform)
			{
				child.localScale = new Vector3(Mathf.Min(v * 2, 1), Mathf.Max((v * 2) - 1, .1f), 1);
			}
			yield return null;
		}
		foreach (Transform child in transform)
		{
			child.localScale = Vector3.one;
		}
	}

	public static IEnumerator AnimateOutro(Transform transform)
	{
		float duration = .5f;
		for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
		{
			float v = 1f - t / duration;
			foreach (Transform child in transform)
			{
				child.localScale = new Vector3(Mathf.Min(v * 2, 1), Mathf.Max((v * 2) - 1, .1f), 1);
			}
			yield return null;
		}
		foreach (Transform child in transform)
		{
			child.localScale = Vector3.zero;
		}
		Destroy(transform.gameObject);
	}

}
