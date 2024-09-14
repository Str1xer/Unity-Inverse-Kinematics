using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AInverseKinematicsActor : MonoBehaviour
{
    [SerializeField]
    public GameObject targetActor;
    public float learningRate = 20f;
    public float samplingDistance = 10f;
    public int maxIterationsCount = 100000;
    public float distanceThreshold = 0.1f;

    private Vector3 targetLocation;
    private Vector3 previousTickTargetLocation;
    private bool bIsTargetReached = false;
    private int iterationIndex = 0;

    private AJoint[] joints;

    void Start()
    {
        joints = GetComponentsInChildren<AJoint>();
    }

    void FixedUpdate()
    {
        targetLocation = targetActor.transform.position;

        if (targetLocation == previousTickTargetLocation)
        {
            iterationIndex++;
        }   
        else
        {
            iterationIndex = 0;
            bIsTargetReached = false;
        }

        if (!bIsTargetReached && iterationIndex < maxIterationsCount)
        { 
            Vector3[] jointsPosition = new Vector3[joints.Length];
            float[] angles = new float[joints.Length];

            for (int i = 0; i <  joints.Length; i++)
            {
                jointsPosition[i] = joints[i].transform.position;
                angles[i] = joints[i].angle;
            }

            for (int i = 0; i < joints.Length; i++)
            {
                float gradient = PartialGradient(targetLocation, angles, i);
                angles[i] -= learningRate * gradient;

                angles[i] = Mathf.Clamp(angles[i], joints[i].minAngle, joints[i].maxAngle);

                joints[i].angle = angles[i];
            }

            if (DistanceFromTarget(targetLocation, angles) < 0.01)
            {
                bIsTargetReached = true;
            }
                
        }

        previousTickTargetLocation = targetLocation;
    }

    private float DistanceFromTarget(Vector3 target, float[] angles)
    {
        Vector3 effectorPosition = Vector3.zero;
        for (int i = angles.Length - 1; i >= 0 ; i--)
        {
            effectorPosition += new Vector3(0, joints[i].boneLength, 0);
            Vector3 direction = joints[i].rotationAxis == Axis.x ? Vector3.right : (joints[i].rotationAxis == Axis.y ? Vector3.up : Vector3.right);
            effectorPosition = Quaternion.AngleAxis(angles[i], direction ) * effectorPosition;
        }
        return Vector3.Distance(transform.position + effectorPosition, target);
    }

    private float PartialGradient (Vector3 target, float[] angles, int i)
    {
        float angle = angles[i];
        float f_x = DistanceFromTarget(target, angles);
        angles[i] += samplingDistance;
        float f_x_Affected = DistanceFromTarget(target, angles);
        float gradient = (f_x_Affected - f_x) / samplingDistance;
        angles[i] = angle;
        return gradient;
    }
}
