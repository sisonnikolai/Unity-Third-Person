using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHangingState : PlayerBaseState
{
    private readonly int HangingHash = Animator.StringToHash("Ledge-Idle");
    private Vector3 ledgeForward;
    private Vector3 closestPoint;
    private const float TransitionDuration = 0.1f;

    public PlayerHangingState(PlayerStateMachine stateMachine, Vector3 ledgeForward, Vector3 closestPoint) : base(stateMachine)
    {
        this.ledgeForward = ledgeForward;
        this.closestPoint = closestPoint;
    }

    public override void Enter()
    {
        stateMachine.transform.rotation = Quaternion.LookRotation(ledgeForward);
        stateMachine.CharacterController.enabled = false;
        stateMachine.transform.position = closestPoint - (stateMachine.LedgeDetector.transform.position - stateMachine.transform.position);
        stateMachine.CharacterController.enabled = true;
        stateMachine.Animator.CrossFadeInFixedTime(HangingHash, TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.MovementValue.y < 0f)
        {
            //stateMachine.CharacterController.Move(Vector3.zero);
            stateMachine.ForceReceiver.Reset();
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
        } else if (stateMachine.InputReader.MovementValue.y > 0f)
        {
            stateMachine.SwitchState(new PlayerPullUpState(stateMachine));
        }
    }

    public override void Exit()
    {
    }

}
