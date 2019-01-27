using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class AIController : MonoBehaviour
{
    private const float CLOSE_DISTANCE = 20;
    private const float TARGET_DISTANCE = 4;
    private const float CLOSE_ANGLE = 0.35f;
    private const float SHOOT_DISTANCE = 10;
    private const float SQR_CLOSE_DISTANCE = CLOSE_DISTANCE * CLOSE_DISTANCE;
    private const float SQR_TARGET_DISTANCE = TARGET_DISTANCE * TARGET_DISTANCE;
    private const float SQR_SHOOT_DISTANCE = SHOOT_DISTANCE * SHOOT_DISTANCE;

    private const int MULTITOOL_GUN = 0;
    private const int MULTITOOL_GATHER = 1;
    private const int MULTITOOL_COMMS = 2;

    public enum Personality { Gatherer, Social, Hostile, Pest }
    private enum Task { HarvestFromPlanet, StealFromShip, TalkToShip, DestroyShip }
    private Task currentTask;

    private float speed = 0.7f;

    private RotateMe rotateMe;

    private float aiDecisionPeriod = 0.5f;
    private float remainingTimeToAiDecision = 0;

    private Planet targetPlanet;
    private Ship targetShip;

    private int forgivenessValue = 5;
    private Dictionary<Ship, int> enemies = new Dictionary<Ship, int>();

    private MultiTool multiTool;

    private void Start()
    {
        rotateMe = GetComponentInChildren<RotateMe>();
        var keyboardCommand = GetComponent<KeyboardCommand>();
        if (keyboardCommand != null)
            keyboardCommand.enabled = false;
        multiTool = GetComponentInChildren<MultiTool>();
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
            if (targetShip != null)
            {
                GetCloserOrActivateMultiTool(targetShip.transform.position, SQR_SHOOT_DISTANCE);
                multiTool.Target = targetShip.transform.position;
            }
        }
        if (currentTask == Task.HarvestFromPlanet)
        {
            if (targetPlanet != null)
            {
                GetCloserOrActivateMultiTool(targetPlanet.transform.position, CLOSE_DISTANCE);
                multiTool.Target = targetPlanet.transform.position;
            }
        }
        else if (currentTask == Task.StealFromShip)
        {
            if (targetShip != null)
            {
                GetCloserOrActivateMultiTool(targetShip.transform.position, CLOSE_DISTANCE);
                multiTool.Target = targetShip.transform.position;
            }
        }
        else if (currentTask == Task.TalkToShip)
        {
            if (targetShip != null)
            {
                GetCloserOrActivateMultiTool(targetShip.transform.position, CLOSE_DISTANCE);
                multiTool.Target = targetShip.transform.position;
            }
        }

    }

    private void DoAI()
    {
        foreach (Ship ship in enemies.Keys.ToList())
        {
            enemies[ship] -= forgivenessValue;
            if (enemies[ship] <= 0)
                enemies.Remove(ship);
        }

        if (enemies.Count > 0)
        {
            currentTask = Task.DestroyShip;
            if (multiTool.SelectedTool != MULTITOOL_GUN)
                multiTool.UpdateSelectedTool(MULTITOOL_GUN);
            targetShip = GetMostHatedShip();
        }
        else if (Random.value < 0.5f)
        {
            currentTask = Task.HarvestFromPlanet;
            if (multiTool.SelectedTool != MULTITOOL_GATHER)
                multiTool.UpdateSelectedTool(MULTITOOL_GATHER);
            targetPlanet = GetBestResourcePlanet(transform.position);
        }
        else if (Random.value < 0.5f)
        {
            currentTask = Task.StealFromShip;
            if (multiTool.SelectedTool != MULTITOOL_GATHER)
                multiTool.UpdateSelectedTool(MULTITOOL_GATHER);
            targetShip = GetBestResourceShip(transform.position);
        }
        else
        {
            currentTask = Task.TalkToShip;
            if (multiTool.SelectedTool != MULTITOOL_COMMS)
                multiTool.UpdateSelectedTool(MULTITOOL_COMMS);
            targetShip = GetBestResourceShip(transform.position);
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

    private static Ship GetBestResourceShip(Vector3 source)
    {
        Ship[] ships = GameState.Instance.Ships;

        Ship bestShip = null;
        float bestScore = 0;
        foreach (Ship ship in ships)
        {
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

    private Ship GetMostHatedShip()
    {
        Ship mostHatedShip = null;
        float highestScore = 0;
        foreach (Ship ship in enemies.Keys)
        {
            if (ship == null)
                continue;
            Vector3 toTarget = ship.transform.position - transform.position;
            float sqrToTargetDistance = toTarget.sqrMagnitude;
            float score = enemies[ship] / sqrToTargetDistance;

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
        int hatred = 0;

        if (enemies.ContainsKey(ship))
        {
            hatred = enemies[ship];
            enemies.Remove(ship);
        }
        enemies.Add(ship, hatred + 100);
        DoAI();
    }
}