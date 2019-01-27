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
        _tool = GetComponentInChildren<MultiTool>();
        _ship = GetComponentInChildren<RotateMe>();
    }

    // Update is called once per frame
    void Update()
    {
        _ship.ThrustPos = Input.GetAxis("Horizontal");
        _ship.ThrustVel = Input.GetAxis("Vertical");
        _tool.Target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKey("1"))
        {
            _tool.SelectedTool = 0;
        }

        if (Input.GetKey("2"))
        {
            _tool.SelectedTool = 1;
        }

        if (Input.GetKey("3"))
        {
            _tool.SelectedTool = 2;
        }

        if (Input.GetMouseButtonDown(1))
        {
            _tool.SelectedTool = _tool.SelectedTool + 1;
        }

        if (Input.GetMouseButton(0))
        {
            _tool.ActivateMultiTool();
        }
    }
}