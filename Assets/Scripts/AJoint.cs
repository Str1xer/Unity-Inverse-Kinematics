using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public enum Axis
{
    x, y, z
}

public class AJoint : MonoBehaviour
{
    [SerializeField]
    [Header("Joint Settings")]
    public Axis rotationAxis;
    public float angle = 0f;
    public float minAngle = -180f;
    public float maxAngle = 180f;

    [SerializeField]
    [Header("Bone Information")]
    public float boneLength = 0f;

    private Vector3 startRotation = Vector3.zero;

    void Start()
    {
        startRotation = transform.localRotation.eulerAngles;
    }

    void Update()
    {
        UpdateRotation();
    }

    void UpdateRotation()
    {
        if (rotationAxis == Axis.x) 
            transform.localRotation = Quaternion.Euler(angle, startRotation.y, startRotation.z);
        if (rotationAxis == Axis.y)
            transform.localRotation = Quaternion.Euler(startRotation.x, angle, startRotation.z);
        if (rotationAxis == Axis.z)
            transform.localRotation = Quaternion.Euler(startRotation.x, startRotation.y, angle);
    }
}
