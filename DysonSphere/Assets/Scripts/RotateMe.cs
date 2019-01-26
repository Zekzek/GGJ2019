using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMe : MonoBehaviour
{
    private float _maxRotSpeed = 200f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float translation = Input.GetAxis("Horizontal") * _maxRotSpeed;
        Vector3 point = new Vector3(0, 0, 0);


        Vector3 axis;
        if(translation > 0)
            axis = new Vector3(0, 0, -1);
        else
            axis = new Vector3(0, 0, 1);

        transform.RotateAround(point, axis, Time.deltaTime * Mathf.Abs(translation));
    }
}
