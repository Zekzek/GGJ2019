using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AIController : MonoBehaviour
{
    private const float CLOSE_DISTANCE = 20;
    private const float TARGET_DISTANCE = 4;
    private const float CLOSE_ANGLE = 0.35f;
    private const float SQR_CLOSE_DISTANCE = CLOSE_DISTANCE * CLOSE_DISTANCE;
    private const float SQR_TARGET_DISTANCE = TARGET_DISTANCE * TARGET_DISTANCE;

    public enum Personality { Gatherer, Social, Hostile, Pest }
    private enum Task { HarvestFromPlanet, StealFromShip, TalkToShip }
    private Task currentTask;

    private float speed = 0.7f;

    private RotateMe rotateMe;

    private float aiDecisionPeriod = 5f;
    private float remainingTimeToAiDecision = 0;

    private Planet targetPlanet;
    private Ship targetShip;

    private void Start()
    {
        rotateMe = GetComponentInChildren<RotateMe>();
        var keyboardCommand = GetComponent<KeyboardCommand>();
        if (keyboardCommand != null)
            keyboardCommand.enabled = false;
    }

    private void Update()
    {
        remainingTimeToAiDecision -= Time.deltaTime;
        if (remainingTimeToAiDecision <= 0)
        {
            remainingTimeToAiDecision = aiDecisionPeriod * Random.Range(0.5f, 1.5f);
            DoAI();
        }

        if (currentTask == Task.HarvestFromPlanet)
        {
            if (targetPlanet != null)
                Go(targetPlanet.transform.position);
        }
        else if (currentTask == Task.StealFromShip)
        {
            if (targetShip != null)
                Go(targetShip.transform.position);
        }
    }

    private void DoAI()
    {
        if (Random.value < 0.5f)
        {
            currentTask = Task.HarvestFromPlanet;
            targetPlanet = GetBestResourcePlanet(transform.position);
        }
        else
        {
            currentTask = Task.StealFromShip;
            targetShip = GetBestResourceShip(transform.position);
        }
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
}