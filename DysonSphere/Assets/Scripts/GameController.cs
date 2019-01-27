using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public GameObject planetPrefab;
    public GameObject aiShipPrefab;

    public Transform planetWrapper;
    public Transform aiShipWrapper;

    void Start()
    {
        for (int i = 0; i < 11; i++)
        {
            float x = 10 * (i - 5) + Random.Range(1, 9);
            float y = Random.Range(-50, 50);
            Instantiate(planetPrefab, new Vector3(x, y, 0), Quaternion.identity, planetWrapper);
        }

        for (int i = 0; i < 11; i++)
        {
            float x = 10 * (i - 5) + Random.Range(1, 9);
            float y = Random.Range(-50, 50);
            Instantiate(aiShipPrefab, new Vector3(x, y, 0), Quaternion.identity, aiShipWrapper);
        }

        GameState.Instance.player.OnResourceChange?.Invoke();
    }
}
