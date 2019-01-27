using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Planet : MonoBehaviour
{
    private static List<Planet> planets = new List<Planet>();
    public static Vector3[] PlanetPositions { get { return planets.Select(x => x.transform.position).ToArray(); } }
    public static Planet[] Planets { get { return planets.ToArray(); } }

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
        planets.Add(this);
    }

    void Update()
    {
        foreach (PlanetResource resource in planetResources)
            resource.Update(Time.deltaTime);
    }

    public Resource TakeResource(float amount)
    {
        int count = planetResources.Count();
        int resourceIndex = Random.Range(0, count);
        return planetResources[resourceIndex].harvest(amount);
    }
}
