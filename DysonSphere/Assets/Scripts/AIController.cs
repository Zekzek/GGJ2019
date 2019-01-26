using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{

    private void Update()
    {
        Debug.Log(GetClosestPlanet(transform.position));
    }

    private Vector3 GetClosestPlanet(Vector3 source)
    {
        return GetClosest(source, Planet.PlanetPositions);
    }

    private Vector3 GetClosest(Vector3 source, Vector3[] targets)
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

    private void Go(Vector3 target)
    {

    }
}