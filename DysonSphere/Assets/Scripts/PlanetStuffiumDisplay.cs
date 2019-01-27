using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetStuffiumDisplay : MonoBehaviour
{
    Text _resourceDisplay;
    Planet _myPlanet;

    void Awake()
    {
        _resourceDisplay = GetComponent<Text>();
        _myPlanet = GetComponentInParent<Planet>();
    }

    void Update()
    {
        _resourceDisplay.text = "x<b>" + Mathf.FloorToInt(_myPlanet.TotalResources) + "</b>";
    }
}
