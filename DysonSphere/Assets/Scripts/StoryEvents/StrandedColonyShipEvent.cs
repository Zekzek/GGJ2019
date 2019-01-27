using System;
using System.Collections.Generic;

public class StrandedColonyShipEvent : ChoiceEvent
{
	public StrandedColonyShipEvent()
	{
        title = "A Request for Aid";
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
