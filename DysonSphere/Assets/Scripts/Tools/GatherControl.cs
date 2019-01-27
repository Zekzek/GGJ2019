using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherControl : ToolControl
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
                parentShip.AddRandomResource(ship.TakeResource(50).amount);
            }
            else if (planet != null)
            {
                parentShip.AddRandomResource(planet.TakeResource(50).amount);
            }
        }
    }
}
