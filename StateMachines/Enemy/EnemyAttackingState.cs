using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyBaseState
{
    private readonly int AttackHash = Animator.StringToHash("Sword-Attack-R1");
    private const float TransitionDuration = 0.1f;
    private string[] Attacks = { "Sword-Attack-R1", "Sword-Attack-R3", "Sword-Attack-R6" };
    public EnemyAttackingState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        int attackIndex = Random.Range(0, 2);
        var attack = stateMachine.Weapon.Attacks[attackIndex];
        FacePlayer();
        //stateMachine.WeaponDamage.SetAttack(stateMachine.AttackDamage, stateMachine.WeaponKnockback);
        stateMachine.Weapon.SetAttack(attack.Damage, attack.Knockback);
        stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, TransitionDuration);
    }

    public override void Tick(float deltaTime)
    {
        if (GetNormalizedTime(stateMachine.Animator, "Attack") >= 1f)
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine));
        }
        //FacePlayer();
    }

    public override void Exit()
    {
    }

}
