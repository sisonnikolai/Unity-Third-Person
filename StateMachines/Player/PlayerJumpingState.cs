using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    private readonly int JumpHash = Animator.StringToHash("Armed-Jump");
    private const float TransitionDuration = 0.1f;
    private Vector3 momentum;

    public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.ForceReceiver.Jump(stateMachine.JumpForce);

        momentum = stateMachine.CharacterController.velocity;
        momentum.y = 0;

        stateMachine.Animator.CrossFadeInFixedTime(JumpHash, TransitionDuration);
        stateMachine.LedgeDetector.OnLedgeDetect += stateMachine.HandleLedgeDetect;
    }

    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);

        if (stateMachine.CharacterController.velocity.y <= 0)
        {
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
        }

        FaceTarget();
    }

    public override void Exit()
    {
        stateMachine.LedgeDetector.OnLedgeDetect -= stateMachine.HandleLedgeDetect;
    }

}
