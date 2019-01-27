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

    public void RemovePlanet(Planet planet)
    {
        planets.Remove(planet);
    }
    public void RemoveShip(Ship ship)
    {
        ships.Remove(ship);
    }

    public PlayerState player = new PlayerState();

    public bool CheckWin()
    {
        bool win = false;
        bool over = false;

        if (player.Ship == null || player.Ship.Health <= 0)
        {
            win = false;
            over = true;
        }
        else
        {
            bool allFriends = true;
            foreach (Ship ship in ships)
            {
                if (!ship.PlayerShip() && ship.PlayerRelationShip != AIController.RelationshipStatus.Ally)
                {
                    allFriends = false;
                    break;
                }
            }
            if (allFriends)
            {
                win = true;
                over = true;
            }
        }

        if (over)
        {
            if (win)
            {
                PopupManager.Show(Resources.Load<VictoryPopup>("UI/Victory Popup"));
            }
            else
            {
                PopupManager.Show(Resources.Load<DefeatPopup>("UI/Defeat Popup"));
            }

            Debug.Log("Game over!!!");
        }

        return over;
    }
}
