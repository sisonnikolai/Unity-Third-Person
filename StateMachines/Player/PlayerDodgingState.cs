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
    private bool isTargeting;

    public PlayerDodgingState(PlayerStateMachine stateMachine, Vector3 dodgeDirectionInput, bool isTargeting = true) : base(stateMachine)
    {
        this.dodgeDirectionInput = dodgeDirectionInput;
        this.isTargeting = isTargeting;
    }

    public override void Enter()
    {
        remainingDodgeDuration = stateMachine.DodgeDuration;

        if (isTargeting)
        {
            stateMachine.Animator.SetFloat(DodgeForwardHash, dodgeDirectionInput.y);
            stateMachine.Animator.SetFloat(DodgeRightHash, dodgeDirectionInput.x);
        }
        else
        {
            stateMachine.Animator.SetFloat(DodgeForwardHash, 1f);
        }

        stateMachine.Animator.CrossFadeInFixedTime(DodgeBlendTreeHash, CrossFadeDuration);

        stateMachine.Health.SetInvulnerable(true);
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement;

        if (isTargeting)
        {
            movement = TargetingDodge();
        }
        else
        {
            movement = FreeLookDodge();
        }

        Move(movement, deltaTime);
        FaceTarget();

        remainingDodgeDuration -= deltaTime;

        if (remainingDodgeDuration <= 0f)
        {
            //stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            ReturnToLocomotion();
        }
    }

    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);
    }

    private Vector3 TargetingDodge()
    {
        Vector3 movement = new Vector3();
        movement += stateMachine.transform.right * dodgeDirectionInput.x * stateMachine.DodgeDistance / stateMachine.DodgeDuration;
        movement += stateMachine.transform.forward * dodgeDirectionInput.y * stateMachine.DodgeDistance / stateMachine.DodgeDuration;

        return movement;
    }

    private Vector3 FreeLookDodge()
    {
        Vector3 movement = CalculateMovement();
        movement += stateMachine.transform.forward * 1f * stateMachine.DodgeDistance / stateMachine.DodgeDuration;

        return movement;
    }
}
