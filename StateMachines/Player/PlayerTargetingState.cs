using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    //private Vector2 dodgeInput;
    //private float remainingDodgeDuration;

    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetSpeedXHash = Animator.StringToHash("TargetSpeedX");
    private readonly int TargetSpeedYHash = Animator.StringToHash("TargetSpeedY");
    private const float AnimatorDampTime = 0.08f;
    private const float CrossFadeDuration = 0.1f;
    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) {}

    public override void Enter()
    {
        stateMachine.InputReader.TargetEvent += OnTarget;
        stateMachine.InputReader.DodgeEvent += OnDodge;
        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        //var attackStamina = stateMachine.Attacks[0].StaminaUsage;
        var attackStamina = stateMachine.WeaponHandler.Weapon.Attacks[0].StaminaUsage;

        if (stateMachine.InputReader.IsAttacking && stateMachine.Stamina.GetStamina() >= attackStamina)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }
        if (stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            return;
        }
        if (stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }
        Vector3 movement = CalculateMovement(deltaTime);
        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);
        UpdateAnimator(deltaTime);
        FaceTarget();
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnTarget;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
        stateMachine.InputReader.JumpEvent -= OnJump;
    }

    private void OnTarget()
    {
        stateMachine.Targeter.CancelTarget();
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    private void OnDodge()
    {
        if (stateMachine.InputReader.MovementValue == Vector2.zero) { return; }
        stateMachine.SwitchState(new PlayerDodgingState(stateMachine, stateMachine.InputReader.MovementValue));
    }
    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
    }

    private Vector3 CalculateMovement(float deltaTime)
    {
        Vector3 movement = new Vector3();

        movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
        movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;

        return movement;
    }

    private void UpdateAnimator(float deltaTime)
    {
        var movement = stateMachine.InputReader.MovementValue;
        if (movement == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(TargetSpeedXHash, 0f, AnimatorDampTime, deltaTime);
            stateMachine.Animator.SetFloat(TargetSpeedYHash, 0f, AnimatorDampTime, deltaTime);
            return;
        }
        stateMachine.Animator.SetFloat(TargetSpeedXHash, movement.x, AnimatorDampTime, deltaTime);
        stateMachine.Animator.SetFloat(TargetSpeedYHash, movement.y, AnimatorDampTime, deltaTime);
    }
}
