using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Planet : MonoBehaviour, IScannable
{
    public float TotalResources
    {
        get
        {
            float total = 0;
            foreach (PlanetResource resource in planetResources)
            {
                total += resource.Harvestable;
            }
            return total;
        }
    }

    [SerializeField]
    private PlanetResource[] planetResources;

    void Start()
    {
        GameState.Instance.AddPlanet(this);
    }

    void Update()
    {
        foreach (PlanetResource resource in planetResources)
            resource.Update(Time.deltaTime);
		transform.localScale = Vector3.one * Mathf.Sqrt(((TotalResources+100) / 100f));
    }

    public Resource TakeResource(float amount)
    {
        int count = planetResources.Count();
        int resourceIndex = Random.Range(0, count);
        return planetResources[resourceIndex].harvest(amount);
    }

    public void ScanMe()
    {
        // Pop up a display that shows resources.
    }
}
