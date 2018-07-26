using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class PlayerMovementInputSys : ComponentSystem
{

    struct Filter
    {
        public PlayerMovementInputData playerMovementInput;
    }

    protected override void OnUpdate()
    {
        float axisX = Input.GetAxis("Horizontal");
        float axisY = Input.GetAxis("Vertical");

        float mX = Input.GetAxis("Mouse X");
        float mY = Input.GetAxis("Mouse Y");

        foreach (var entity in GetEntities<Filter>())
        {
            entity.playerMovementInput.horizontalAxis = axisX;
            entity.playerMovementInput.verticalAxis = axisY;

            entity.playerMovementInput.mouseX = mX;
            entity.playerMovementInput.mouseY = mY;

            entity.playerMovementInput.jump = Input.GetKey(entity.playerMovementInput.jumpKey);
        }
    }
}
