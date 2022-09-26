using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        //stateMachine.Ragdoll.ToggleRagdoll(true);
        stateMachine.Animator.CrossFadeInFixedTime("Armed-Death", 0.1f);  // TODO: set different death animations (sword, bow, staff)
        //stateMachine.WeaponDamage.gameObject.SetActive(false);
        stateMachine.WeaponHandler.Weapon.gameObject.SetActive(false);
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
    }

}
