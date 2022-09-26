using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPullUpState : PlayerBaseState
{
    private readonly int PullUpHash = Animator.StringToHash("Ledge-Climb");
    private readonly Vector3 Offset = new Vector3(0f, 2.325f, 0.65f);
    private const float TransitionDuration = 0.1f;

    public PlayerPullUpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(PullUpHash, TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        //if (stateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f) { return; } // wait till the animation is done before switching to the freelook state
        if (GetNormalizedTime(stateMachine.Animator, "Climb") < 1f) { return; }
        stateMachine.CharacterController.enabled = false;
        stateMachine.transform.Translate(Offset, Space.Self);
        stateMachine.CharacterController.enabled = true;
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine, false));
    }

    public override void Exit()
    {
        stateMachine.CharacterController.Move(Vector3.zero);
        stateMachine.ForceReceiver.Reset();
    }

}
