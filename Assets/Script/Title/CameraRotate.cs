using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float RotationSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, RotationSpeed * Time.deltaTime, 0f);
    }
}
