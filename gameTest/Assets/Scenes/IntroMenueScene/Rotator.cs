using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed = 1f;
    public Vector3 rotationAxis = new Vector3(0,1,0);

    // Update is called once per frame
    void Update()
    {

        transform.RotateAround(transform.position, rotationAxis, rotationSpeed*Time.deltaTime);

    }
}
