using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardCommand : MonoBehaviour
{
    private RotateMe ship;
    // Start is called before the first frame update
    void Start()
    {
        ship = GetComponent<RotateMe>();
    }

    // Update is called once per frame
    void Update()
    {
        ship.ThrustPos = Input.GetAxis("Horizontal");
        ship.ThrustVel = Input.GetAxis("Vertical");
    }
}
