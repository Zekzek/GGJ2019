using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnrestEvent : ChoiceEvent
{
    public UnrestEvent()
    {
        title = "Crew Moral Conflict";

        scenario = "Your crew has become unhappy and are making demands!";

        options = new List<Option>()
        {
            {
                "Reject demands",
                new ChanceResults<Func<string>>()
                    {
                        {
                            10,
                            ()=> {
                                GameState.Instance.player.Unrest++;
                                return "That could have gone better... unrest in your crew increases.";
                             }
                        },
                        {
                            20,
                            () => {
                                GameState.Instance.player.Unrest = Math.Min(0, GameState.Instance.player.Unrest - 1);
                                return "You respectfully explain your position and your crew empathizes.";
                            }
                        },
                        {
                            100,
                            () => {
                                GameState.Instance.player.Resources = 17;
                                int resources_thrown = 20;
                                int old_resource_num = (int) GameState.Instance.player.Resources;

                                GameState.Instance.player.Unrest++;
                                GameState.Instance.player.Resources = Math.Max(0, GameState.Instance.player.Resources - resources_thrown);
                                int amount_resources_lost = (int) old_resource_num - (int) GameState.Instance.player.Resources;

                                return string.Format("Your crew throws {0} resources off the ship in revolt!", amount_resources_lost);
                            }
                        }
                    }.PickOne()
            },
            {
                "Give into their demands (-10 Resources)",
                new ChanceResults<Func<string>>()
                    {
                        {
                            10,
                            ()=> {
                                GameState.Instance.player.Unrest+= 10;
                                GameState.Instance.player.Resources = Math.Min(0, GameState.Instance.player.Resources - 10);
                                return "Despite agreeing to their terms, the crew's unrest increases.";
                             }
                        },
                        {
                            50,
                            ()=>{
                                GameState.Instance.player.Unrest = Math.Min(0, GameState.Instance.player.Unrest - 1);
                                GameState.Instance.player.Resources = Math.Min(0, GameState.Instance.player.Resources - 10);
                                return "Your crew is satisifed and unrest is decreased.";
                            }
                        }
                    }.PickOne()
            },
        };
    }

}
