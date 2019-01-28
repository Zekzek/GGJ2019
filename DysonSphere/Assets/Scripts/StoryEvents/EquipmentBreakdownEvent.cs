using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentBreakdownEvent : ChoiceEvent
{
    public EquipmentBreakdownEvent()
        {
            title = "Maintenance Needed";
            scenario = "There is a problem with your propulsion system!";

            options = new List<Option>()
            {
                {
                    "Hope the problem fixes itself",
                    new ChanceResults<Func<string>>()
                        {
                            {
                                10,
                                ()=> {
                                    RotateMe rm = GameState.Instance.player.Ship.GetComponentInChildren<RotateMe>();
                                    rm._maxVel = Math.Max(1, rm._maxVel - 2);
                                    return "Surprisingly, that didn't work.";
                                 }
                            },
                            {
                                10,
                                ()=> {
                                    GameState.Instance.player.Unrest += 2;
                                    RotateMe rm = GameState.Instance.player.Ship.GetComponentInChildren<RotateMe>();
                                    rm._maxVel = Math.Max(1, rm._maxVel - 2);

                                    return "That didn't work and your crew becomes resentful of your inaction.";
                                 }
                            },
                            {
                                5,
                                ()=> {
                                    GameState.Instance.player.Unrest -= 1;
                                    return "Wow, that actually worked!";
                                 }
                            }
                        }.PickOne()
                },
                {
                    "Repair",
                    new ChanceResults<Func<string>>()
                        {
                            {
                                10,
                                ()=> {
                                    // Reduce speed
                                    GameState.Instance.player.Resources = Math.Max(0, GameState.Instance.player.Resources - 10);
                                    RotateMe rm = GameState.Instance.player.Ship.GetComponentInChildren<RotateMe>();
                                    rm._maxVel = Math.Max(1, rm._maxVel - 0.05f);

                                    return "Minor speed loss, unfortunately duct tape doesn't solve all problems.";
                                 }
                            },
                            {
                                10,
                                ()=>{
                                    return "Repairs are successful, no loss to navigation speed";
                                }
                            },
                            {
                                5,
                                ()=>{
                                    // Increase to speed
                                    RotateMe rm = GameState.Instance.player.Ship.GetComponentInChildren<RotateMe>();
                                    rm._maxVel += 2;

                                    return "Repairs successful and system improved.";
                                }
                            }
                        }.PickOne()
                },
            };
        }

}
