using UnityEngine;
using System.Collections;

public class OrbitCamera : MonoBehaviour
{
    public float speed;
    public static bool ctrlClick = false;
    private float currentAngle = 0;

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (ctrlClick && Input.GetKey(KeyCode.LeftControl)
                || !ctrlClick)
            {
                currentAngle = Input.GetAxis("Mouse X") * speed * Time.deltaTime * -1;
                transform.RotateAround(Vector3.up, currentAngle);
            }
        }

    }
}