using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public PlanetPresetDB planetDB;
    public SpriteRenderer[] landDetailSpr;
    public SpriteRenderer[] waterSpr;
    public SpriteRenderer[] windSpr;
    private List<Resource> resources = new List<Resource>();

    public float TotalResources
    {
        get
        {
            float total = 0;
            foreach (Resource resource in resources)
            {
                total += resource.amount;
            }
            return total;
        }
    }

    private void Start()
    {
        setPreset();

        GameState.Instance.AddShip(this);
        resources.Add(new Resource(Resource.Type.Stuffium, 100));
    }

    public void AddRandomResource(float amount)
    {
        int resourceIndex = Random.Range(0, resources.Count);
        resources[resourceIndex].amount += amount;
    }

    public Resource TakeResource(float amount)
    {
        int resourceIndex = Random.Range(0, resources.Count);
        if (resources[resourceIndex].amount < amount)
        {
            amount = 0;
            resources[resourceIndex].amount = 0;
        }
        resources[resourceIndex].amount -= amount;

        return new Resource(resources[resourceIndex].type, amount);
    }

    private void setPreset()
    {
        int i = Random.Range(0, planetDB.planets.Length);
        var landSpr = GetComponent<SpriteRenderer>();

        landSpr.color = planetDB.planets[i].landColor;

        windSpr[1].transform.parent.GetComponent<ScrollMaterial>().speed = planetDB.planets[i].windspeed;
        landDetailSpr[1].transform.parent.GetComponent<ScrollMaterial>().speed = planetDB.planets[i].spinspeed;

        foreach (SpriteRenderer sp in landDetailSpr)
        {
            sp.color = planetDB.planets[i].landDetailColor;
        }

        foreach (SpriteRenderer sp in waterSpr)
        {
            sp.color = planetDB.planets[i].waterColor;
        }

        foreach (SpriteRenderer sp in windSpr)
        {
            sp.color = planetDB.planets[i].cloudColor;
        }

    }
}
