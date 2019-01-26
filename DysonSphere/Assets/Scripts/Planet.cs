using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Planet : MonoBehaviour
{
    private static List<Planet> planets = new List<Planet>();
    public static Vector3[] PlanetPositions { get { return planets.Select(x => x.transform.position).ToArray(); } }

    void Start()
    {
        planets.Add(this);
    }

    void Update()
    {

    }
}
