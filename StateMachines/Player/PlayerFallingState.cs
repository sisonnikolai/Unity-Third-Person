using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    private readonly int FallHash = Animator.StringToHash("Armed-Fall");
    private const float TransitionDuration = 0.1f;
    private Vector3 momentum;

    public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        momentum = stateMachine.CharacterController.velocity;
        momentum.y = 0;

        stateMachine.Animator.CrossFadeInFixedTime(FallHash, TransitionDuration);
        stateMachine.LedgeDetector.OnLedgeDetect += stateMachine.HandleLedgeDetect;
    }

    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);

        if (stateMachine.CharacterController.isGrounded)
        {
            ReturnToLocomotion();
        }

        FaceTarget();
    }

    public override void Exit()
    {
        stateMachine.LedgeDetector.OnLedgeDetect -= stateMachine.HandleLedgeDetect;
    }

}
