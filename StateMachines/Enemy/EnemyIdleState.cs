using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private readonly int MovementHash = Animator.StringToHash("Movement");
    private readonly int UnequipHash = Animator.StringToHash("UnequipToBack");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    private bool shouldUnequip;
    public EnemyIdleState(EnemyStateMachine stateMachine, bool shouldUnequip = false) : base(stateMachine)
    {
        this.shouldUnequip = shouldUnequip;
    }

    public override void Enter()
    {
        if (shouldUnequip)
        {
            stateMachine.Animator.CrossFadeInFixedTime(UnequipHash, CrossFadeDuration);
        }
        else
        {
            stateMachine.Animator.CrossFadeInFixedTime(MovementHash, CrossFadeDuration);
        }
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);
        if (IsInRange())
        {
            //stateMachine.SwitchState(new EnemyChasingState(stateMachine));
            stateMachine.WeaponHandler.SetWeaponEquip(true);
            stateMachine.SwitchState(new EnemyEquipState(stateMachine));
            return;
        }
        stateMachine.Animator.SetFloat(SpeedHash, 0f, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
    }

}
