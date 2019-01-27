using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : ToolControl
{
    public GameObject bullet, muzzleFlash;

    public Transform[] GunEnds;

    public override void DoActivate()
    {
        Debug.Log("GUN");
        foreach (Transform t in GunEnds)
        {
            if(t.name == "GunEnd")
            {
                Instantiate(bullet, t.transform.position, transform.parent.rotation);
                Instantiate(muzzleFlash, t.transform);
            }
        }
    }
}
