using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GatherControl : ToolControl
{
    GameObject i;
    public LineRenderer line;
    public GameObject GatherTxt;

    public override void DoActivate()
    {
        if (parentShip != null)
        {
            Ship ship;
            Planet planet;
            GetInFront(new Vector2(2, 5), out ship, out planet);
            if (ship != null)
            {
                var newTxt = Instantiate(GatherTxt, transform.parent.position, Quaternion.identity);

                if (newTxt != null)
                    Destroy(newTxt, 1);

                var amount = ship.TakeResource(50, parentShip).amount;
                parentShip.AddRandomResource(amount);

                newTxt.transform.GetChild(0).GetComponent<Text>().text = "+" + Mathf.RoundToInt(amount);

                i = (ship.gameObject);
            }
            else if (planet != null)
            {
                var newTxt = Instantiate(GatherTxt, transform.parent.position, Quaternion.identity);

                if (newTxt != null)
                    Destroy(newTxt, 1);

                var amount = planet.TakeResource(50).amount;
                parentShip.AddRandomResource(amount);

                newTxt.transform.GetChild(0).GetComponent<Text>().text = "+" + Mathf.RoundToInt(amount);

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
            i = ship.gameObject;
            line.gameObject.SetActive(true);
        }
        else if (planet != null)
        {
            i = planet.gameObject;
            line.gameObject.SetActive(true);
        }
        else
        {
            i = null;
            line.gameObject.SetActive(false);
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
