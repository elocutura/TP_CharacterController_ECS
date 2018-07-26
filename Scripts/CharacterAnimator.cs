using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public static readonly int SPEED = Animator.StringToHash("Speed");
    public static readonly int HORIZONTAL_SPEED = Animator.StringToHash("HorizontalSpeed");
    public static readonly int VERTICAL_SPEED = Animator.StringToHash("VerticalSpeed");
    public static readonly int IS_GROUNDED = Animator.StringToHash("IsGrounded");
    public static readonly int IS_SLIDING = Animator.StringToHash("IsSliding");
    public static readonly int IDLE = Animator.StringToHash("Idle");

    private Animator animator;
    public CharacterControllerData characterControllerData;
    public CharacterController cc;

    protected virtual void Awake()
    {
        this.animator = this.GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        this.animator.SetFloat(SPEED, this.characterControllerData.CurrentSpeed);
        this.animator.SetFloat(HORIZONTAL_SPEED, this.characterControllerData.RawInput.x);
        this.animator.SetFloat(VERTICAL_SPEED, this.characterControllerData.RawInput.y);
        this.animator.SetBool(IS_GROUNDED, this.cc.isGrounded);
        this.animator.SetBool(IS_SLIDING, this.characterControllerData.IsSliding);
    }

    public void FootR()
    {
        //Debug.Log("Anim Right foot down");
    }
    public void FootL()
    {
        //Debug.Log("Anim Left foot down");
    }
    public void Jump()
    {
        //Debug.Log("Anim Jump");
    }
    public void Land()
    {
        //Debug.Log("Anim Landed");
    }
}
