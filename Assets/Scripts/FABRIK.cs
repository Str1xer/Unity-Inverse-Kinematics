using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FABRIK : MonoBehaviour
{
    [SerializeField]
    public GameObject targetActor;

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

        if (!bIsTargetReached)
        {
            Vector3[] jointsPosition = new Vector3[joints.Length + 1];
            for (int i = 0; i <  joints.Length; i++)
            {
                jointsPosition[i] = joints[i].transform.position;
            }

            Vector3 rootPosition = jointsPosition[0];
            float[] distances = new float[jointsPosition.Length];
            float[] ratio = new float[jointsPosition.Length];

            jointsPosition[jointsPosition.Length - 1] = targetLocation;

            for (int i = jointsPosition.Length - 2; i >= 0; i--)
            {
                UpdateJointPosition(joints[i], ref jointsPosition[i], jointsPosition[i + 1]);
            }

            jointsPosition[0] = rootPosition;
            for (int i = 0; i < jointsPosition.Length - 1; i++)
            {
                UpdateJointPosition(joints[i], ref jointsPosition[i + 1], jointsPosition[i]);
            }


            for (int i = 0; i < jointsPosition.Length - 1; i++)
            { 
                Debug.DrawLine(jointsPosition[i], jointsPosition[i + 1], Color.green, 0.1f, false);
            }

        }

        previousTickTargetLocation = targetLocation;
    }

    private void UpdateJointPosition(AJoint joint, ref Vector3 jointPosition, Vector3 targetPosition)
    {
        float distances = Vector3.Distance(jointPosition, targetPosition);
        float ratio = joint.boneLength/distances;
        jointPosition = (1 - ratio) * targetPosition  + ratio * jointPosition;
    }
}
