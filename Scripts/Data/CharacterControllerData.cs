using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class CharacterControllerData : MonoBehaviour
{

    private Vector2 rawInput;
    public Vector2 RawInput { get { return rawInput; } set { rawInput = value; } }
    public float RawInputX { set { rawInput.x = value; } }
    public float RawInputY { set { rawInput.y = value; } }
    private Vector3 move;
    public Vector3 Move { get { return move; } set { move = value; } }
    public float MoveX { set { move.x = value; } }
    public float MoveY { set { move.y = value; } }
    public float MoveZ { set { move.z = value; } }
    private bool isGrounded = false;
    public bool IsGrounded { get { return isGrounded; } set { isGrounded = value; } }
    private bool isJumping = false;
    public bool IsJumping { get { return isJumping; } set { isJumping = value; } }
    private bool isSliding = false;
    public bool IsSliding { get { return isSliding; } set { isSliding = value; } }
    private float currentSpeed = 0;
    public float CurrentSpeed { get { return currentSpeed; } set { currentSpeed = value; } }
    public float speed;
    public float acceleration;
    public float jumpForce;
    public float gravityValue = 9.6f;
    public float airResistance = 0.01f;

    public Transform cameraPivot;

}
