using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommsControl : ToolControl
{
    public override void DoActivate()
    {
        if (parentShip != null)
        {
            Ship ship;
            Planet planet;
            GetInFront(new Vector2(2, 5), out ship, out planet);
            if (ship != null)
            {
                parentShip.TalkTo(ship);
            }
            else if (planet != null)
            {
                parentShip.TalkTo(planet);
            }
        }
    }
}
