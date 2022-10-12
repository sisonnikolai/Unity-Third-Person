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

    public EnemyEquipState(EnemyStateMachine stateMachine, float timeBetweenActions) : base(stateMachine)
    {
        this.timeBetweenActions = timeBetweenActions;
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(EquipHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        // if next action is ready then attack
        //timeBetweenActions -= Time.deltaTime;
        //if (timeBetweenActions <= 0f) // TODO: enemy attacks even though player is not in range
        //{
            stateMachine.SwitchState(new EnemyChasingState(stateMachine, timeBetweenActions));
        //    return;
        //}
    }

    public override void Exit()
    {
    }

}
