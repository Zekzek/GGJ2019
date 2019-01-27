using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private List<Resource> resources = new List<Resource>();

    private void Start()
    {
        resources.Add(new Resource(Resource.Type.Stuffium, 100));
    }

    public Resource TakeResource(float amount)
    {
        int count = resources.Count;
        int resourceIndex = Random.Range(0, count);
        if (resources[resourceIndex].amount < amount)
        {
            amount = 0;
            resources[resourceIndex].amount = 0;
        }
        resources[resourceIndex].amount -= amount;

        return new Resource(resources[resourceIndex].type, amount);
    }
}
