using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
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

    public void TalkTo(Ship ship)
    {
        //TODO
    }

    public void TalkTo(Planet planet)
    {
        //TODO
    }
}
