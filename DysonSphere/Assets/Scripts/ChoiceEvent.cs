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

	public static ChoiceEvent GenerateRandomChoiceEvent(GameState gameState)
	{
		return new StrandedColonyShipEvent();
	}

	protected class ChanceResults : IEnumerable
	{
		List<Tuple<float, Func<string>>> possibilities = new List<Tuple<float, Func<string>>>();

		public IEnumerator GetEnumerator()
		{
			return null;
		}

		public void Add(float chance, Func<string> OnChoosen)
		{
			possibilities.Add(new Tuple<float, Func<string>>(chance, OnChoosen));
		}

		public Func<string> PickOne()
		{
			float f = UnityEngine.Random.Range(0, possibilities.Sum(p => p.Item1));
			Func<string> result = () => "";
			foreach (Tuple<float, Func<string>> possibility in possibilities)
			{
				f -= possibility.Item1;
				if (f < 0)
					break;

				result = possibility.Item2;
			}
			return result;
		}
	}
}


public class StrandedColonyShipEvent : ChoiceEvent
{
	public StrandedColonyShipEvent()
	{
		scenario = "You discover a stranded colony ship asking for aid. Do you :";

		options.Add(new Option(
			"Ignore them",
			() =>
			{
				return "You ignore them";
			}));

		options.Add(new Option(
			"Destroy them",
			() =>
			{
				return new ChanceResults()
				{
					{
						100,
						()=>
						{
							int damage = UnityEngine.Random.Range(1,300);
							PlayerState player = GameState.Instance.player;
							int newHealth = player.Health - damage;

							player.Health = Math.Max(1,newHealth);
							if(newHealth <= 100)
							{
								return "They return fire, nearly destroying your ship.";
							}
							else
							{
								player.Health = newHealth;
					 			return "They return fire, but they were no match for you.";
							}
					 	}
					},
					{
						100,
						() =>
						{
					 			return "They are unable to return fire and are destroyed.";
						}

					}
				}.PickOne().Invoke();
			}));

		options.Add(new Option(
			"Assist them",
			() =>
			{
				return new ChanceResults()
				{
					{
						10,
						()=>
						{
							int damage = UnityEngine.Random.Range(1,600);
							PlayerState player = GameState.Instance.player;
							int newHealth = player.Health - damage;

							player.Health = Math.Max(1,newHealth);
							if(newHealth <= 100)
							{
								return "It was a Trap. They nearly destroy your ship.";
							}
							else
							{
								player.Health = newHealth;
					 			return "It was a Trap, but they were no match for you.";
							}
					 	}
					},
					{
						100,
						() =>
						{
					 		
							PlayerState player = GameState.Instance.player;
							player.Resources += UnityEngine.Random.Range(100,600);
							return "They are grateful and give you resources.";
						}

					}
				}.PickOne().Invoke();
			}));

	}
}