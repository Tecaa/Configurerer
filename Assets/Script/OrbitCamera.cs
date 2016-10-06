﻿using UnityEngine;
using System.Collections;

public class OrbitCamera : MonoBehaviour
{
    public float speed;
    public static bool active = true;
    private float currentAngle = 0;

    void Update()
    {
        if (active && Input.GetButton("Fire1"))
        {
            
            currentAngle = Input.GetAxis("Mouse X") * speed * Time.deltaTime * -1;
            transform.RotateAround(Vector3.up, currentAngle);
        }

    }
}