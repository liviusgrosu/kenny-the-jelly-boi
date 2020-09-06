using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObject : MonoBehaviour
{
    public bool x, y, z;
    public float speed;

    public bool floatUpAndDown;
    Vector3 pointA, pointB;

    private void Start() {
        pointA = transform.position;
        pointB = new Vector3(transform.position.x, transform.position.y + Random.Range(0.1f, 0.3f), transform.position.z);
    }

    private void Update() {
        //Spin the object depending on what axis the user requested in edit mode
        if (x) transform.Rotate(speed, 0, 0);
        if (y) transform.Rotate(0, speed, 0);
        if (z) transform.Rotate(0, 0, speed);

        if(floatUpAndDown) transform.position = Vector3.Lerp(pointA, pointB, Mathf.PingPong(Time.time, 1));
    }
}
