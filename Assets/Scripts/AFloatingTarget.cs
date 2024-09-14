using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFloatingTarget : MonoBehaviour
{
    [SerializeField]
    public GameObject actor;

    private AJoint[] joints;

    void Start()
    {
        joints = actor.GetComponentsInChildren<AJoint>();
    }

    void Update()
    {
        Vector3 effectorPosition = Vector3.zero;
        for (int i = joints.Length - 1; i >= 0 ; i--)
        {
            effectorPosition += new Vector3(0, joints[i].boneLength, 0);
            Vector3 direction = joints[i].rotationAxis == Axis.x ? Vector3.right : (joints[i].rotationAxis == Axis.y ? Vector3.up : Vector3.right);
            effectorPosition = Quaternion.AngleAxis(joints[i].angle, direction ) * effectorPosition;
        }
        transform.position = actor.transform.position + effectorPosition;
    }
}
