using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArmedIdleState : EnemyBaseState
{
    private readonly int ArmedHash = Animator.StringToHash("Armed-Idle");
    private const float CrossFadeDuration = 0.1f;
    private float timeBetweenActions = 0f;

    public EnemyArmedIdleState(EnemyStateMachine stateMachine, float timeBetweenActions) : base(stateMachine)
    {
        this.timeBetweenActions = timeBetweenActions;
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ArmedHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        // if next action is ready then attack
        timeBetweenActions -= Time.deltaTime;
        if (timeBetweenActions <= 0f) // TODO: enemy attacks even though player is not in range
        {
            stateMachine.SwitchState(new EnemyChasingState(stateMachine, timeBetweenActions));
            return;
        }
    }

    public override void Exit()
    {
    }

}
