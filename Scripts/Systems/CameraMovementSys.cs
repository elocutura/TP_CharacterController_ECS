using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class CameraMovementSys : ComponentSystem {

    struct Filter
    {
        public MainCameraMovementData mainCamera;
    }

    protected override void OnStartRunning()
    {
        base.OnStartRunning();

        Cursor.lockState = CursorLockMode.Locked;
    }

    protected override void OnUpdate()
    {
        foreach (var entity in GetEntities<Filter>())
        {
            var controlRotation = GetMouseRotationInput(3f, entity.mainCamera.Target);
            this.UpdateRotation(entity.mainCamera, controlRotation);

            FollowTarget(entity.mainCamera); // Needs to be placed on lateUpdate when that comes out for ECS
        }
    }

    private static float lookAngle = 0f;
    private static float tiltAngle = 0f;

    private Quaternion GetMouseRotationInput(float mouseSensitivity, PlayerMovementInputData playerInput)
    {

        // Adjust the look angle (Y Rotation)
        lookAngle += playerInput.mouseX * mouseSensitivity;
        lookAngle %= 360f;

        // Adjust the tilt angle (X Rotation)
        tiltAngle += playerInput.mouseY * mouseSensitivity;
        tiltAngle %= 360f;
        tiltAngle = MathfExtensions.ClampAngle(tiltAngle, -80f, 30f);

        var controlRotation = Quaternion.Euler(-tiltAngle, lookAngle, 0f);
        return controlRotation;
    }

    private void UpdateRotation(MainCameraMovementData mainCam, Quaternion controlRotation)
    {
        if (mainCam.Target != null)
        {
            // Y Rotation (Look Rotation)
            mainCam.RigTargetLocalRotation = Quaternion.Euler(0f, controlRotation.eulerAngles.y, 0f);

            // X Rotation (Tilt Rotation)
            mainCam.PivotTargetLocalRotation = Quaternion.Euler(controlRotation.eulerAngles.x, 0f, 0f);

            if (mainCam.RotationSmoothing > 0.0f)
            {
                mainCam.Pivot.localRotation =
                    Quaternion.Slerp(mainCam.Pivot.localRotation, mainCam.PivotTargetLocalRotation, mainCam.RotationSmoothing * Time.deltaTime);

                mainCam.Rig.localRotation =
                    Quaternion.Slerp(mainCam.Rig.localRotation, mainCam.RigTargetLocalRotation, mainCam.RotationSmoothing * Time.deltaTime);
            }
            else
            {
                mainCam.transform.localRotation = mainCam.PivotTargetLocalRotation;
                mainCam.transform.localRotation = mainCam.RigTargetLocalRotation;
            }
        }
    }

    private void FollowTarget(MainCameraMovementData mainCam)
    {
        Vector3 camVel = mainCam.CameraVelocity;

        if (mainCam.Target == null)
        {
            return;
        }

        mainCam.Rig.position = Vector3.SmoothDamp(mainCam.Rig.position, mainCam.Target.transform.position, ref camVel, mainCam.CatchSpeedDamp);

        mainCam.CameraVelocity = camVel;
    }
}
