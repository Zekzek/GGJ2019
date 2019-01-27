using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipStuffiumDisplay : MonoBehaviour
{
    Text _resourceDisplay;
    Ship _myShip;

    void Awake()
    {
        _resourceDisplay = GetComponent<Text>();
        _myShip = GetComponentInParent<Ship>();
    }

    void Update()
    {
        _resourceDisplay.text = "x<b>" + Mathf.FloorToInt(_myShip.TotalResources) + "</b>";
    }
}
