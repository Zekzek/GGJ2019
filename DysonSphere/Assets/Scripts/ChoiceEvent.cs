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

public class StrandedColonyShipEvent : ChoiceEvent
{
	public StrandedColonyShipEvent()
	{
		scenario = "You discover a stranded colony ship asking for aid.";

		options = new List<Option>()
		{
			{
				"Ignore them",
				new ChanceResults<Func<string>>()
				{
					{
						10,
						()=> {
							int damage = UnityEngine.Random.Range(1,300);
							PlayerState player = GameState.Instance.player;
							int newHealth = player.Health - damage;

							player.Health = Math.Max(1,newHealth);
							if(newHealth <= 100)
							{
								return "They opened fire, nearly destroying your ship, but you managed to escaped";
							}
							else
							{
								player.Health = newHealth;
								return "They opened fire, but you managed to escaped.";
							}
					 	}
					},
					{
						100,
						() => {
					
							return "You ignore them";
						}

					}
				}.PickOne()
			},
			{
				"Destroy them",
				new ChanceResults<Func<string>>()
				{
					{
						100,
						()=> {
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
							() => {
						 			return "They are unable to return fire and are destroyed.";
							}

					}
				}.PickOne()
			},
			{
				"Assist them",
				new ChanceResults<Func<string>>()
				{
					{
						10,
						()=> {
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
							50,
							() => {
								return "They are grateful.";
							}
						},
						{
							50,
							() => {

							PlayerState player = GameState.Instance.player;
							player.Resources += UnityEngine.Random.Range(100,600);
							return "They are grateful and give you resources.";
						}
					}
				}.PickOne()
			}
		};
	}
}

public class NewIdeaEvent : ChoiceEvent
{
	private class Upgrade
	{
		public string scenarioItem;


		public Func<string> attemptOption;
	}

	public NewIdeaEvent()
	{
		Upgrade upgrade = new ChanceResults<Upgrade>()
			{
				{
					1f/(Mathf.Max(GameState.Instance.player.GatherDistance,1)),
					new Upgrade()
					{
						scenarioItem = "Resource Gatherer",
						attemptOption = ()=>
						{
							GameState.Instance.player.GatherDistance += UnityEngine.Random.Range(1,5);
							return "It works! You can now garther from further away!";
						}
					}
				},
				{
					1f/(Mathf.Max(GameState.Instance.player.CommunicationDistance,1)),
					new Upgrade()
					{
						scenarioItem = "Communication Array",
						attemptOption = ()=>
						{
							GameState.Instance.player.CommunicationDistance += UnityEngine.Random.Range(1,5);
							return "It works! You can now comunicate from further away!";
						}
					}
				}
			}.PickOne();

		scenario = string.Format("One of your crew has an idea to imrove the {0}.", upgrade.scenarioItem);

		options = new List<Option>()
		{
			{
				"Regect the idea",
				new ChanceResults<Func<string>>()
					{
						{
							10,
							()=> {
								GameState.Instance.player.Unrest++;
								return "The crew thinks you are too cautious.";
						 	}
						},
						{
							20,
							() => {
								GameState.Instance.player.Unrest--;
								return "The crew thinks you were wise to not risk an upgrade.";
							}
						}
					}.PickOne()
			},
			{
				"Allow them to attempt the upgrade",
				new ChanceResults<Func<string>>()
					{
						{
							10,
							()=> {
								GameState.Instance.player.Unrest+= 10;
								return "The upgrade nearly overloaded the ship.";
						 	}
						},
						{
							10,
							upgrade.attemptOption
						},
						{
							50,
							()=>{ return "Nothing seems to have changed."; }
						}
					}.PickOne()
			},
		};
	}
}
