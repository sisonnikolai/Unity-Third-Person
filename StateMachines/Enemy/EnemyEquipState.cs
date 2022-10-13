using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyEquipState : EnemyBaseState
{
    private readonly int EquipHash = Animator.StringToHash("EquipFromBack");
    private const float CrossFadeDuration = 0.1f;
    private float timeBetweenActions = 0f;

    public EnemyEquipState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(EquipHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        stateMachine.SwitchState(new EnemyChasingState(stateMachine, timeBetweenActions));
    }

    public override void Exit()
    {
    }

}
