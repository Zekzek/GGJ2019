using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
	List<Popup> popups = new List<Popup>();

	private static PopupManager instance;
	public static PopupManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<PopupManager>();
				if (instance == null)
				{
					GameObject go = new GameObject("PopupManager");
					instance = go.AddComponent<PopupManager>();
				}
			}

			return instance;
		}
	}

	public static T Show<T>(T popupPrefab)
		where T : Popup
	{
		T inst = Instantiate(popupPrefab);
		Instance.popups.Add(inst);
		Instance.StartCoroutine(AnimateIntro(inst.transform));
		return inst;
	}

	public static void Close(Popup popup)
	{
		Instance.StartCoroutine(AnimateOutro(popup));
	}

	private void Update()
	{
		for(int i = 0; i < popups.Count;) 
		{
			if (popups[i] == null)
			{
				popups.RemoveAt(i);
				continue;
			}
			i++;
		}
		Time.timeScale = (popups.Any()) ? 0 : 1;
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

	public static IEnumerator AnimateOutro(Popup popup)
	{
		float duration = .5f;
		for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
		{
			float v = 1f - t / duration;
			foreach (Transform child in popup.transform)
			{
				child.localScale = new Vector3(Mathf.Min(v * 2, 1), Mathf.Max((v * 2) - 1, .1f), 1);
			}
			yield return null;
		}
		foreach (Transform child in popup.transform)
		{
			child.localScale = Vector3.zero;
		}
		Instance.popups.Remove(popup);
		Destroy(popup.gameObject);
	}
}
