using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommsControl : ToolControl
{
    protected int resourceCost = 5;

    public override void DoActivate()
    {
        if (parentShip != null && parentShip.TotalResources >= resourceCost)
        {
            Ship ship;
            Planet planet;
            GetInFront(new Vector2(2, 10), out ship, out planet);
            if (ship != null)
            {
                ship.Appreciate(parentShip);
                parentShip.TalkTo(ship);
                GetComponentInChildren<Animator>().Play("RadioWave");
                if (!GetComponent<AIController>())
                {
                    ship.transform.Find("BasicStatHUD").GetComponent<Animator>().Play("ShowStuffium");
                }
            }
            else if (planet != null)
            {
                parentShip.TalkTo(planet);
                GetComponentInChildren<Animator>().Play("RadioWave");
                if (!GetComponent<AIController>())
                {
                    planet.transform.Find("BasicStatStuffiumHUD").GetComponent<Animator>().Play("ShowStuffium");
                }
            }
            parentShip.TakeResource(resourceCost, null);
        }
    }
}
