using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    enum RotationAxis
    {
        X,
        Y,
        Z
    }
    [SerializeField] private RotationAxis rotationAxis;
    private Vector3 privateRotation;
    [SerializeField] private float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        switch (rotationAxis)
        {
            case RotationAxis.X:
                privateRotation = Vector3.right;
                break;
            case RotationAxis.Y:
                privateRotation = Vector3.up;
                break;
            case RotationAxis.Z:
                privateRotation = Vector3.forward;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(privateRotation, rotationSpeed * Time.deltaTime);
    }
}
