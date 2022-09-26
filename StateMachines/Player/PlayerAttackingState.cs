using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingState : PlayerBaseState
{
    private Attack attackAnimation;
    private float previousFrameTime;
    private bool alreadyAppliedForce;

    public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine) 
    {
        //attackAnimation = stateMachine.Attacks[attackIndex];
        attackAnimation = stateMachine.WeaponHandler.Weapon.Attacks[attackIndex];
    }

    public override void Enter()
    {
        //stateMachine.WeaponDamage.SetAttack(attackAnimation.Damage, attackAnimation.Knockback);
        stateMachine.WeaponHandler.Weapon.SetAttack(attackAnimation.Damage, attackAnimation.Knockback);
        stateMachine.Animator.CrossFadeInFixedTime(attackAnimation.AnimationName, attackAnimation.TransitionDuration);
        stateMachine.Stamina.ReduceStamina(attackAnimation.StaminaUsage);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        FaceTarget();

        float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");

        if (normalizedTime >= previousFrameTime && normalizedTime < 1f)
        {
            if (normalizedTime >= attackAnimation.ForceTime)
            {
                TryApplyForce();
            }

            if (stateMachine.InputReader.IsAttacking)
            {
                TryComboAttack(normalizedTime);
            }
        }
        else
        {
            if (stateMachine.Targeter.CurrentTarget != null)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }

        previousFrameTime = normalizedTime;
    }

    public override void Exit()
    {
    }

    
    private void TryComboAttack(float normalizedTime)
    {
        if (attackAnimation.ComboStateIndex == -1) { return; }
        if (normalizedTime < attackAnimation.ComboAttackTime) { return; }
        if (stateMachine.Stamina.GetStamina() < attackAnimation.StaminaUsage) { return; }

        stateMachine.SwitchState(new PlayerAttackingState(stateMachine, attackAnimation.ComboStateIndex));
    }

    private void TryApplyForce()
    {
        if (alreadyAppliedForce) { return; }
        stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attackAnimation.Force);
        alreadyAppliedForce = true;
    }

}
