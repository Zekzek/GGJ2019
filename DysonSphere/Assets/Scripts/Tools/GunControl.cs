using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : ToolControl
{
    protected int resourceCost = 25;

    public GameObject bullet, muzzleFlash;
    public float spreadAmount;
    public Transform[] GunEnds;
    public int barrelIndex = 0;
    private AudioSource aud;
    public AudioClip outClip;

    public override void DoActivate()
    {
        if (parentShip != null)
        {
            if (parentShip.TotalResources >= resourceCost)
            {
                Shoot(GunEnds[barrelIndex]);
                parentShip.TakeResource(resourceCost, null);
            }
            else
            {
                if (PlayerShip())
                {
                    aud = GetComponent<AudioSource>();
                    aud.pitch = Random.Range(0.5f, 1.5f);
                    aud.PlayOneShot(outClip);
                }
            }
        }
    }

    public void Shoot(Transform t)
    {
        var newbullet = Instantiate(bullet, t.position, transform.parent.rotation);
        newbullet.transform.Rotate(0, 0, Random.Range(-spreadAmount, spreadAmount));

        Bullet bulletBullet = newbullet.GetComponent<Bullet>();
        bulletBullet.parentShip = parentShip;

        var newFlash = Instantiate(muzzleFlash, t.position, transform.parent.rotation, t);
        var m = newFlash.GetComponent<ParticleSystem>().main;
        m.startRotation = transform.parent.rotation.eulerAngles.y - 90 * Mathf.Deg2Rad;

        barrelIndex += 1;
        if (barrelIndex == GunEnds.Length)
        {
            barrelIndex = 0;
        }
    }
}
