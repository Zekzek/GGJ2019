using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ChoiceEvent
{
	public class Option
	{
		public string option;
		public Func<string> OnChoosen;

		public Option() { }
		public Option(string option, Func<string> OnChoosen)
		{
			this.option = option;
			this.OnChoosen = OnChoosen;
		}
	}

	public string scenario;
	public List<Option> options;

	public ChoiceEvent() { options = new List<Option>(); }
	public ChoiceEvent(string scenario, IEnumerable<Option> options)
	{
		this.scenario = scenario;
		this.options = new List<Option>(options);
	}

	public static ChoiceEvent GenerateRandomChoiceEvent()
	{
		return new ChanceResults<ChoiceEvent>()
		{
			{
				1,
				new StrandedColonyShipEvent()
			},
			{
				1,
				new NewIdeaEvent()
			},
            {
                (float) Math.Pow(GameState.Instance.player.Unrest / 10, 2),
                new UnrestEvent()
            }
		}.PickOne();
	}

	protected class ChanceResults<T> : IEnumerable
	{
		List<Tuple<float, T>> possibilities = new List<Tuple<float, T>>();

		public IEnumerator GetEnumerator()
		{
			return null;
		}

		public void Add(float chance, T v)
		{
			possibilities.Add(new Tuple<float, T>(chance, v));
		}

		public T PickOne()
		{
			float f = UnityEngine.Random.Range(0, possibilities.Sum(p => p.Item1));
			T result = default(T);
			foreach (Tuple<float, T> possibility in possibilities)
			{
				result = possibility.Item2;
				f -= possibility.Item1;
				if (f < 0)
					break;
			}
			return result;
		}
	}
}
public static class ListChoiceEventOption_EXT
{
	public static void Add(this List<ChoiceEvent.Option> options, string optionText, Func<string> OnChoosen)
	{
		options.Add(new ChoiceEvent.Option(optionText, OnChoosen));
	}
}
