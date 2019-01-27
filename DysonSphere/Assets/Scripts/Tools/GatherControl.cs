using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherControl : ToolControl
{
    GameObject i;
    public LineRenderer line;

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

                i = (ship.gameObject);
            }
            else if (planet != null)
            {
                parentShip.AddRandomResource(planet.TakeResource(50).amount);

                i = (planet.gameObject);
            }
            else
            {
                i = (null);
            }
        }
    }

    void Update()
    {
        Ship ship;
        Planet planet;
        GetInFront(new Vector2(2, 5), out ship, out planet);
        if (ship != null)
        {
            i = (ship.gameObject);
        }
        else if (planet != null)
        {
            i = (planet.gameObject);
        }
        else
        {
            i = (null);
        }
        if (i != null)
        {
            line.gameObject.SetActive(true);
            var dist = line.GetPosition(0) - i.transform.position;
            //var curPos = line.GetPosition(1);
            line.SetPosition(1, Vector3.up * Vector3.Distance(transform.position, i.transform.position));
        }
        else
        {
            line.SetPosition(1, line.GetPosition(0));
        }
    }

}
