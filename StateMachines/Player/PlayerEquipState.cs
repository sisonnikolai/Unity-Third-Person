using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipState : PlayerBaseState
{
    private readonly int EquipHash = Animator.StringToHash("EquipFromBack");
    private readonly int UnequipHash = Animator.StringToHash("UnequipToBack");
    private readonly int EquipMoveSpeedHash = Animator.StringToHash("EquipMoveSpeed");
    private readonly int EquipBlendTreeHash = Animator.StringToHash("EquipBlendTree");
    private const float AnimatorDampTime = 0.1f;
    private const float CrossFadeDuration = 0.1f;
    public PlayerEquipState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.InputReader.EquipEvent += OnEquip;
        stateMachine.Animator.CrossFadeInFixedTime(EquipBlendTreeHash, CrossFadeDuration);
    }


    public override void Tick(float deltaTime)
    {
        var attackStamina = stateMachine.WeaponHandler.weapon.Attacks[0].StaminaUsage;

        if (stateMachine.InputReader.IsAttacking && stateMachine.Stamina.GetValue() >= attackStamina)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }

        Vector3 movement = CalculateMovement();

        Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);

        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(EquipMoveSpeedHash, 0f, AnimatorDampTime, deltaTime);
            return;
        }
        stateMachine.Animator.SetFloat(EquipMoveSpeedHash, stateMachine.InputReader.MovementValue.magnitude, AnimatorDampTime, deltaTime);
        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.InputReader.EquipEvent -= OnEquip;
    }

    private void OnEquip()
    {
        stateMachine.WeaponHandler.SetWeaponEquip(false);
        stateMachine.Animator.CrossFadeInFixedTime(UnequipHash, CrossFadeDuration);
    }

    private void FaceMovementDirection(Vector3 movement, float deltaTime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp(stateMachine.transform.rotation, Quaternion.LookRotation(movement), stateMachine.RotationDamping * deltaTime);
    }
}
