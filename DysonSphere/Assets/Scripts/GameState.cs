using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private static GameState instance;

    private List<Planet> planets = new List<Planet>();
    public Vector3[] PlanetPositions { get { return planets.Select(x => x.transform.position).ToArray(); } }
    public Planet[] Planets { get { return planets.ToArray(); } }
    private List<Ship> ships = new List<Ship>();
    public Ship[] Ships { get { return ships.ToArray(); } }

    public static GameState Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameState>();
                if (instance == null)
                {
                    GameObject go = new GameObject("GameStateSingleton");
                    instance = go.AddComponent<GameState>();
                }
            }

            return instance;
        }
    }

    public void AddPlanet(Planet planet)
    {
        planets.Add(planet);
    }

    public void AddShip(Ship ship)
    {
        ships.Add(ship);
    }

    public PlayerState player = new PlayerState();
}
