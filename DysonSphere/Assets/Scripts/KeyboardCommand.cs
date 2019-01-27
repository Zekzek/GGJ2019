using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardCommand : MonoBehaviour
{
    private MultiTool _tool;
    private RotateMe _ship;
    // Start is called before the first frame update
    void Start()
    {
        _tool = GetComponent<MultiTool>();
        _ship = GetComponent<RotateMe>();
    }

    // Update is called once per frame
    void Update()
    {
        _ship.ThrustPos = Input.GetAxis("Horizontal");
        _ship.ThrustVel = Input.GetAxis("Vertical");
        _tool.Target = Input.mousePosition;
    }
}
