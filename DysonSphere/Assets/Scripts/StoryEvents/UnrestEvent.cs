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
                                GameState.Instance.player.Unrest++;
                                GameState.Instance.player.Resources = Math.Min(0, GameState.Instance.player.Resources - 20);
                                return "Your crew throws 100 resources off the ship in revolt!";
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
