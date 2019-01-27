using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject planetPrefab;
    public GameObject aiShipPrefab;

    public Transform planetWrapper;
    public Transform aiShipWrapper;

    private const float MIN_CLUSTER_DISTANCE = 12;
    private const float SQR_MIN_CLUSTER_DISTANCE = MIN_CLUSTER_DISTANCE * MIN_CLUSTER_DISTANCE;

    private int retryCounter;

    void Start()
    {
        List<Vector2> usedPoints = new List<Vector2>();
        usedPoints.Add(Vector2.zero); //Player position

        for (int i = 0; i < 15; i++)
        {
            Vector2 point = Vector2.zero;
            while (point == Vector2.zero || !IsInOpenSpace(usedPoints, point))
                point = Random.Range(0.15f, 1f) * Random.insideUnitCircle * 55;
            usedPoints.Add(point);
            Instantiate(planetPrefab, point, Quaternion.identity, planetWrapper);
        }

        for (int i = 0; i < 9; i++)
        {
            Vector2 point = Vector2.zero;
            while (point == Vector2.zero || !IsInOpenSpace(usedPoints, point))
                point = Random.Range(0.15f, 1f) * Random.insideUnitCircle * 55;
            usedPoints.Add(point);
            Instantiate(aiShipPrefab, point, Quaternion.identity, aiShipWrapper);
        }

        Debug.Log("Placed things on 24 of " + retryCounter + "attempts");
    }

    private bool IsInOpenSpace(List<Vector2> usedPoints, Vector2 prospectivePoint)
    {
        retryCounter++;
        foreach (Vector2 point in usedPoints)
            if ((point - prospectivePoint).sqrMagnitude < SQR_MIN_CLUSTER_DISTANCE)
                return false;
        return true;
    }
}
