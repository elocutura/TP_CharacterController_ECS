using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class MainCameraMovementData : MonoBehaviour {



    // Serializable fields
    [SerializeField]
    private PlayerMovementInputData target = null; // The target to follow
    public PlayerMovementInputData Target { get { return target; } }

    [SerializeField]
    private float catchSpeedDamp = 0f;
    public float CatchSpeedDamp { get { return catchSpeedDamp; } }

    [SerializeField]
    [Tooltip("How fast the camera rotates around the pivot")]
    private float rotationSmoothing = 15.0f;
    public float RotationSmoothing { get { return rotationSmoothing; } }

    // private fields
    [SerializeField]
    private Transform rig; // The root transform of the camera rig
    public Transform Rig { get { return rig; } }

    [SerializeField]
    private Transform pivot; // The point at which the camera pivots around
    public Transform Pivot { get { return pivot; } }

    private Quaternion pivotTargetLocalRotation; // Controls the X Rotation (Tilt Rotation)
    public Quaternion PivotTargetLocalRotation { get { return pivotTargetLocalRotation; } set { pivotTargetLocalRotation = value; } }
    private Quaternion rigTargetLocalRotation; // Controlls the Y Rotation (Look Rotation)
    public Quaternion RigTargetLocalRotation { get { return rigTargetLocalRotation; } set { rigTargetLocalRotation = value; } }
    private Vector3 cameraVelocity; // The velocity at which the camera moves
    public Vector3 CameraVelocity { get { return cameraVelocity; } set { cameraVelocity = value; } }

}
