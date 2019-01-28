using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class AIController : MonoBehaviour
{
    private const float CLOSE_DISTANCE = 20;
    private const float TARGET_DISTANCE = 4;
    private const float CLOSE_ANGLE = 0.35f;
    private const float SHOOT_DISTANCE = 17;
    private const float SQR_CLOSE_DISTANCE = CLOSE_DISTANCE * CLOSE_DISTANCE;
    private const float SQR_TARGET_DISTANCE = TARGET_DISTANCE * TARGET_DISTANCE;
    private const float SQR_SHOOT_DISTANCE = SHOOT_DISTANCE * SHOOT_DISTANCE;
    private const int ENEMY_RELATIONSHIP_LEVEL = -50;
    private const int ALLY_RELATIONSHIP_LEVEL = 20;

    private const int MULTITOOL_GUN = 0;
    private const int MULTITOOL_GATHER = 1;
    private const int MULTITOOL_COMMS = 2;

    private int shotAnger = 100;
    private int theftAnger = 10;
    private int commsJoy = 10;
    private int forgivenessValue = 5;

    private float chanceToRetaliate = 0.75f;
    private float chanceToAttack = 0.05f;
    private float chanceToGather = 0.35f;
    private float chanceToSteal = 0.25f;
    private float chanceToTalk = 0.35f;

    public enum Personality { Default, Gatherer, Social, Hostile, Pest }
    private Personality personality;
    public Personality Temperment
    {
        get { return personality; }
        set
        {
            personality = value;
            if (personality == Personality.Social)
            {
                shotAnger = 50;
                theftAnger = 10;
                commsJoy = 20;
                forgivenessValue = 10;
                chanceToRetaliate = 0.5f;
                chanceToAttack = 0;
                chanceToGather = 0.6f;
                chanceToSteal = 0;
                chanceToTalk = 0.4f;
            }
            else if (personality == Personality.Hostile)
            {
                shotAnger = 200;
                theftAnger = 50;
                commsJoy = 5;
                forgivenessValue = 5;
                chanceToRetaliate = 1;
                chanceToAttack = 0.3f;
                chanceToGather = 0.5f;
                chanceToSteal = 0;
                chanceToTalk = 0.1f;
            }
            else if (personality == Personality.Pest)
            {
                shotAnger = 20;
                theftAnger = 50;
                commsJoy = 5;
                forgivenessValue = 5;
                chanceToRetaliate = 0.5f;
                chanceToAttack = 0f;
                chanceToGather = 0.1f;
                chanceToSteal = 0.8f;
                chanceToTalk = 0.1f;
            }
            else if (personality == Personality.Gatherer)
            {
                shotAnger = 50;
                theftAnger = 50;
                commsJoy = 10;
                forgivenessValue = 7;
                chanceToRetaliate = 0.5f;
                chanceToAttack = 0f;
                chanceToGather = 0.9f;
                chanceToSteal = 0f;
                chanceToTalk = 0.1f;
            }
        }
    }

    private enum Task { HarvestFromPlanet, StealFromShip, TalkToShip, DestroyShip }
    private Task currentTask;

    public enum RelationshipStatus { Ally, Neutral, Enemy }
    public RelationshipStatus PlayerRelationship
    {
        get
        {
            Ship playerShip = GameState.Instance.player.Ship;
            if (!relationships.ContainsKey(playerShip))
                relationships.Add(playerShip, 0);

            int relationship = relationships[playerShip];
            if (relationship <= ENEMY_RELATIONSHIP_LEVEL)
                return RelationshipStatus.Enemy;
            else if (relationship >= ALLY_RELATIONSHIP_LEVEL)
                return RelationshipStatus.Ally;
            else
                return RelationshipStatus.Neutral;
        }
    }

    private float speed = 0.7f;

    private RotateMe rotateMe;

    private float aiDecisionPeriod = 0.5f;
    private float remainingTimeToAiDecision = 0;

    private Planet targetPlanet;
    private Ship targetShip;

    private Dictionary<Ship, int> relationships = new Dictionary<Ship, int>();
	public IDictionary<Ship,int> GetRelationships { get { return relationships; } }

	private MultiTool multiTool;
    private Ship myShip;

    private void Start()
    {
        rotateMe = GetComponentInChildren<RotateMe>();
        var keyboardCommand = GetComponent<KeyboardCommand>();
        if (keyboardCommand != null)
            keyboardCommand.enabled = false;
        multiTool = GetComponentInChildren<MultiTool>();
        Temperment = (Personality)Random.Range(0, 5);
        myShip = GetComponent<Ship>();
    }

    private void Update()
    {
        remainingTimeToAiDecision -= Time.deltaTime;
        if (remainingTimeToAiDecision <= 0)
        {
            remainingTimeToAiDecision = aiDecisionPeriod * Random.Range(0.5f, 1.5f);
            DoAI();
        }

        if (currentTask == Task.DestroyShip)
        {
            if (targetShip == null || !CanShoot())
                DoAI();
            else
            {
                GetCloserOrActivateMultiTool(targetShip.transform.position, SQR_SHOOT_DISTANCE);
                multiTool.Target = targetShip.transform.position;
            }
        }
        else if (currentTask == Task.StealFromShip)
        {
            if (targetShip == null)
                DoAI();
            else
            {
                GetCloserOrActivateMultiTool(targetShip.transform.position, CLOSE_DISTANCE);
                multiTool.Target = targetShip.transform.position;
            }
        }
        else if (currentTask == Task.TalkToShip)
        {
            if (targetShip == null || !CanTalk())
                DoAI();
            else
            {
                GetCloserOrActivateMultiTool(targetShip.transform.position, CLOSE_DISTANCE);
                multiTool.Target = targetShip.transform.position;
            }
        }
        else if (currentTask == Task.HarvestFromPlanet)
        {
            if (targetPlanet != null)
            {
                GetCloserOrActivateMultiTool(targetPlanet.transform.position, CLOSE_DISTANCE);
                multiTool.Target = targetPlanet.transform.position;
            }
        }
    }

    private void DoAI()
    {
        List<Ship> enemies = new List<Ship>();

        foreach (Ship ship in relationships.Keys.ToList())
        {
            if (relationships[ship] < ENEMY_RELATIONSHIP_LEVEL)
            {
                relationships[ship] += forgivenessValue;
                if (relationships[ship] < ENEMY_RELATIONSHIP_LEVEL)
                    enemies.Add(ship);
            }
        }

        float randomValue = Random.value;
        if (enemies.Count > 0 && CanShoot() && Random.value < chanceToRetaliate)
        {
            currentTask = Task.DestroyShip;
            if (multiTool.SelectedTool != MULTITOOL_GUN)
                multiTool.UpdateSelectedTool(MULTITOOL_GUN);
            targetShip = GetMostHatedShip(enemies);
        }
        else if (randomValue < chanceToSteal)
        {
            currentTask = Task.StealFromShip;
            if (multiTool.SelectedTool != MULTITOOL_GATHER)
                multiTool.UpdateSelectedTool(MULTITOOL_GATHER);
            targetShip = GetBestResourceShip(transform.position);
        }
        else if (randomValue < chanceToTalk && CanTalk())
        {
            currentTask = Task.TalkToShip;
            if (multiTool.SelectedTool != MULTITOOL_COMMS)
                multiTool.UpdateSelectedTool(MULTITOOL_COMMS);
            targetShip = GetBestResourceShip(transform.position);
        }
        else if (randomValue < chanceToAttack && CanShoot())
        {
            currentTask = Task.DestroyShip;
            if (multiTool.SelectedTool != MULTITOOL_GUN)
                multiTool.UpdateSelectedTool(MULTITOOL_GUN);
            targetShip = GetBestResourceShip(transform.position);
        }
        else
        {
            currentTask = Task.HarvestFromPlanet;
            if (multiTool.SelectedTool != MULTITOOL_GATHER)
                multiTool.UpdateSelectedTool(MULTITOOL_GATHER);
            targetPlanet = GetBestResourcePlanet(transform.position);
        }
    }

    private void GetCloserOrActivateMultiTool(Vector3 target, float sqrDistance)
    {
        if ((target - transform.position).sqrMagnitude > sqrDistance)
            Go(target);
        else
            multiTool.ActivateMultiTool();
    }

    private void Go(Vector3 target)
    {
        if (target == null)
            return;

        float targetThrustVelocity = 0;

        float sqrDistance = (target - transform.position).sqrMagnitude;
        if (rotateMe.DeltaAngle < CLOSE_ANGLE && rotateMe.DeltaAngle > -CLOSE_ANGLE)
        {
            float offCourseDamper = Mathf.Abs(rotateMe.DeltaAngle) / CLOSE_ANGLE;

            if (sqrDistance > SQR_CLOSE_DISTANCE)
                targetThrustVelocity = offCourseDamper;
            else if (sqrDistance > SQR_TARGET_DISTANCE)
                targetThrustVelocity = (sqrDistance - SQR_TARGET_DISTANCE) / (SQR_CLOSE_DISTANCE - SQR_TARGET_DISTANCE) * offCourseDamper;
        }

        rotateMe.LockOn = true;
        rotateMe.ThrustVel = targetThrustVelocity;
        rotateMe.RotSpeed = 500f;
        rotateMe.Target = target;
    }

    private static Planet GetBestResourcePlanet(Vector3 source)
    {
        Planet[] planets = GameState.Instance.Planets;

        Planet bestPlanet = null;
        float bestScore = 0;
        foreach (Planet planet in planets)
        {
            Vector3 toTarget = planet.transform.position - source;
            float sqrToTargetDistance = toTarget.sqrMagnitude;
            float score = planet.TotalResources / sqrToTargetDistance;

            if (score > bestScore)
            {
                bestPlanet = planet;
                bestScore = score;
            }
        }
        return bestPlanet;
    }

    private Ship GetBestResourceShip(Vector3 source)
    {
        Ship[] ships = GameState.Instance.Ships;

        Ship bestShip = null;
        float bestScore = 0;
        foreach (Ship ship in ships)
        {
            if (relationships.ContainsKey(ship) && relationships[ship] >= ALLY_RELATIONSHIP_LEVEL)
                continue;

            Vector3 toTarget = ship.transform.position - source;
            float sqrToTargetDistance = toTarget.sqrMagnitude;
            float score = ship.TotalResources / sqrToTargetDistance;

            if (score > bestScore)
            {
                bestShip = ship;
                bestScore = score;
            }
        }
        return bestShip;
    }

    private Ship GetMostHatedShip(List<Ship> enemies)
    {
        Ship mostHatedShip = null;
        float highestScore = 0;
        foreach (Ship ship in enemies)
        {
            if (ship == null)
                continue;
            Vector3 toTarget = ship.transform.position - transform.position;
            float sqrToTargetDistance = toTarget.sqrMagnitude;
            float score = -relationships[ship] / sqrToTargetDistance;

            if (score > highestScore)
            {
                mostHatedShip = ship;
                highestScore = score;
            }
        }
        return mostHatedShip;
    }

    private static Vector3 GetDirectionToClosestPlanet(Vector3 source)
    {
        return GetClosest(source, GameState.Instance.PlanetPositions) - source;
    }

    private static Vector3 GetClosest(Vector3 source, Vector3[] targets)
    {
        Vector3 closest = Vector3.zero;
        float closestSquareDistance = float.MaxValue;
        foreach (Vector3 target in targets)
        {
            Vector3 toTarget = target - source;
            float sqrToTargetDistance = toTarget.sqrMagnitude;

            if (sqrToTargetDistance < closestSquareDistance)
            {
                closest = target;
                closestSquareDistance = sqrToTargetDistance;
            }
        }
        return closest;
    }

    public void TookDamageFrom(Ship ship)
    {
        if (!relationships.ContainsKey(ship))
            relationships.Add(ship, 0);

        relationships[ship] -= shotAnger;
        DoAI();
    }

    public void VictimOfTheftFrom(Ship ship)
    {
        if (!relationships.ContainsKey(ship))
            relationships.Add(ship, 0);

        relationships[ship] -= theftAnger;
        DoAI();
    }

    public void Appreciate(Ship ship)
    {
        if (!relationships.ContainsKey(ship))
            relationships.Add(ship, 0);

        relationships[ship] += commsJoy;
        DoAI();
    }

    private bool CanShoot()
    {
        return myShip.TotalResources >= 25;
    }

    private bool CanTalk()
    {
        return myShip.TotalResources >= 5;
    }

}