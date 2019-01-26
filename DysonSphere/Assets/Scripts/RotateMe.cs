using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMe : MonoBehaviour
{
    private float _maxRotSpeed = 200f;

    // Update is called once per frame
    void Update()
    {
        float translation = Input.GetAxis("Horizontal") * _maxRotSpeed;
        
        Vector3 axis;
        if(translation > 0)
            axis = new Vector3(0, 0, -1);
        else
            axis = new Vector3(0, 0, 1);

        transform.RotateAround(transform.parent.position, axis, Time.deltaTime * Mathf.Abs(translation));

        float thrust = Input.GetAxis("Vertical");
        Vector2 direction = transform.localPosition.normalized;
        var prb = transform.parent.GetComponent<Rigidbody2D>();
        prb.AddForce(direction * -10f * thrust);
    }
}
