using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private static GameState instance;

    private List<Planet> planets = new List<Planet>();
    public Vector3[] PlanetPositions { get { return planets.Select(x => x.transform.position).ToArray(); } }
    public Planet[] Planets { get { return planets.ToArray(); } }
    private List<Ship> ships = new List<Ship>();
    public Ship[] Ships { get { return ships.ToArray(); } }
	private List<ShipLog> shipLogs = new List<ShipLog>();

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

	public void AddShipLog(ShipLog shipLog)
	{
		shipLogs.Add(shipLog);
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

	public string GenerateSynopsis()
	{
		StringBuilder sb = new StringBuilder();
		ShipLog[] logs = shipLogs.Concat(ships.Where(s => !s.PlayerShip()).Select(s => s.GenerateShipLog(false))).ToArray();
		int liveAllies = 0;
		int deadAllies = 0;
		int liveEnemies = 0;
		int deadEnemies = 0;
		foreach(ShipLog log in logs)
		{
			switch(log.PlayerRelationship)
			{
				case AIController.RelationshipStatus.Ally:
					{
						if(log.Dead)
						{
							deadAllies++;
						}
						else
						{
							liveAllies++;
						}
					}
					break;
				case AIController.RelationshipStatus.Enemy:
					{
						if (log.Dead)
						{
							deadEnemies++;
						}
						else
						{
							liveEnemies++;
						}
					}
					break;
			}
		}
		sb.AppendLine(string.Format("Allies : {0} Live, {1} Dead", liveAllies, deadAllies));
		sb.AppendLine(string.Format("Enemies : {0} Live, {1} Dead", liveEnemies, deadEnemies));
		sb.AppendLine(string.Format("Unrest : {0}", Instance.player.Unrest));

		return sb.ToString();
	}
}
