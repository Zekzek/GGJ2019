using System;
using System.Collections.Generic;
using UnityEngine;

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
							return "It works! You can now gather from further away!";
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

        title = "Research Opportunity";

		scenario = string.Format("One of your crew has an idea to improve the {0}.", upgrade.scenarioItem);

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
