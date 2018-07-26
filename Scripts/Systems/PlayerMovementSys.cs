using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class PlayerMovementSys : ComponentSystem {

    struct Filter
    {
        public PlayerMovementInputData playerMovementInput;
        public CharacterController cc;
        public CharacterControllerData ccData;
    }

    private Vector3 slopeParallel;

    protected override void OnUpdate()
    {
        foreach (var entity in GetEntities<Filter>())
        {
            handlePlayerInput(entity.playerMovementInput, entity.cc, entity.ccData);
        }
    }

    private void handlePlayerInput(PlayerMovementInputData playerInput, CharacterController cc, CharacterControllerData ccData)
    {
        CheckGround(cc, ccData);
        UpdateMoveDir(playerInput, cc, ccData);
        if (Input.GetKeyDown(KeyCode.Space) && cc.isGrounded)
        {
            ccData.IsJumping = true;
            ccData.MoveY = ccData.jumpForce;
            ccData.MoveX = ccData.Move.x * ccData.jumpForce / 15;
            ccData.MoveZ = ccData.Move.z * ccData.jumpForce / 15;
        }
        if (!ccData.IsSliding)
            cc.Move(ccData.Move * Time.deltaTime);

        rotateWithMouse(cc, ccData);
    }

    protected void rotateWithMouse(CharacterController cc, CharacterControllerData ccData)
    {
        Vector3 eu = ccData.cameraPivot.rotation.eulerAngles;
        cc.transform.rotation = Quaternion.Euler(0, eu.y, eu.z);
    }

    protected void UpdateMoveDir(PlayerMovementInputData playerInput, CharacterController cc, CharacterControllerData ccData)
    {

        float airResistance = ccData.airResistance;

        if (cc.isGrounded && !ccData.IsSliding)
        {
            ccData.RawInputX = playerInput.horizontalAxis;
            ccData.RawInputY = playerInput.verticalAxis;
            ccData.Move = new Vector3(0, 0, 0);

            ccData.CurrentSpeed = ccData.speed * (Mathf.Abs(ccData.RawInput.x) + Mathf.Abs(ccData.RawInput.y));
            if (ccData.CurrentSpeed > ccData.speed)
                ccData.CurrentSpeed = ccData.speed;
            if (ccData.RawInput.y < 0)
                ccData.CurrentSpeed /= 2;

            if (ccData.RawInput.y > 0.1)
            {
                ccData.Move += cc.transform.forward;
            }
            else if (ccData.RawInput.y < -0.1)
            {
                ccData.Move -= cc.transform.forward;
            }

            if (ccData.RawInput.x > 0.1)
            {
                ccData.Move += cc.transform.right;
            }
            else if (ccData.RawInput.x < -0.1)
            {
                ccData.Move -= cc.transform.right;
            }

            if (ccData.RawInput == Vector2.zero)
                ccData.CurrentSpeed = 0;

            ccData.Move *= ccData.CurrentSpeed;
            ccData.MoveY = -ccData.gravityValue;
        }
        else // Air Res
        {
            if (ccData.Move.x > 0)
            {
                ccData.MoveX= ccData.Move.x - airResistance;
            }
            else if (ccData.Move.x < 0)
            {
                ccData.MoveX = ccData.Move.x + airResistance;
            }
            if (ccData.Move.z > 0)
            {
                ccData.MoveZ = ccData.Move.z - airResistance;
            }
            else if (ccData.Move.z < 0)
            {
                ccData.MoveZ = ccData.Move.z + airResistance;
            }
            ccData.MoveY = ccData.Move.y - ccData.gravityValue / 10;
        }

    }

    private void CheckGround(CharacterController cc, CharacterControllerData ccData)
    {
        RaycastHit hit;
        // Raycast with infinite distance to check the slope directly under the player no matter where they are
        Physics.Raycast(cc.transform.position, Vector3.down, out hit, Mathf.Infinity);

        // Saving the normal
        Vector3 n = hit.normal;

        // Crossing my normal with the player's up vector (if your player rotates I guess you can just use Vector3.up to create a vector parallel to the ground
        Vector3 groundParallel = Vector3.Cross(cc.transform.up, n);

        // Crossing the vector we made before with the initial normal gives us a vector that is parallel to the slope and always pointing down
        slopeParallel = Vector3.Cross(groundParallel, n);
        // Just the current angle we're standing on
        float currentSlope = Mathf.Round(Vector3.Angle(hit.normal, cc.transform.up));

        // If the slope is on a slope too steep and the player is Grounded the player is pushed down the slope.
        if (currentSlope >= cc.slopeLimit && cc.isGrounded)
        {
            ccData.IsSliding = true;
            Vector3 move = slopeParallel.normalized * (0 + ccData.gravityValue / 100);
            move.y -= ccData.gravityValue / 10;
            cc.Move(move);
        }
        else
        {
            ccData.IsSliding = false;
        }
    }
}
