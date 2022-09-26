using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{
    private readonly int DodgeBlendTreeHash = Animator.StringToHash("DodgeBlendTree");
    private readonly int DodgeRightHash = Animator.StringToHash("DodgeRight");
    private readonly int DodgeForwardHash = Animator.StringToHash("DodgeForward");
    private const float CrossFadeDuration = 0.1f;

    private Vector3 dodgeDirectionInput;
    private float remainingDodgeDuration;

    public PlayerDodgingState(PlayerStateMachine stateMachine, Vector3 dodgeDirectionInput) : base(stateMachine)
    {
        this.dodgeDirectionInput = dodgeDirectionInput;
    }

    public override void Enter()
    {
        remainingDodgeDuration = stateMachine.DodgeDuration;

        stateMachine.Animator.SetFloat(DodgeForwardHash, dodgeDirectionInput.y);
        stateMachine.Animator.SetFloat(DodgeRightHash, dodgeDirectionInput.x);
        stateMachine.Animator.CrossFadeInFixedTime(DodgeBlendTreeHash, CrossFadeDuration);

        stateMachine.Health.SetInvulnerable(true);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = new Vector3();

        movement += stateMachine.transform.right * dodgeDirectionInput.x * stateMachine.DodgeDistance / stateMachine.DodgeDuration;
        movement += stateMachine.transform.forward * dodgeDirectionInput.y * stateMachine.DodgeDistance / stateMachine.DodgeDuration;

        Move(movement, deltaTime);
        FaceTarget();

        remainingDodgeDuration -= deltaTime;

        if (remainingDodgeDuration <= 0f)
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);
    }

}
