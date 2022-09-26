using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChasingState : EnemyBaseState
{
    private readonly int SpeedHash = Animator.StringToHash("Speed");
    private readonly int MovementHash = Animator.StringToHash("Movement");
    private const float CrossFadeDuration = 0.1f;
    private const float AnimatorDampTime = 0.1f;
    private float timeBetweenActions = 0f;
    public EnemyChasingState(EnemyStateMachine stateMachine, float timeBetweenActions = 0f) : base(stateMachine)
    {
        if (timeBetweenActions == 0f) { timeBetweenActions = Random.Range(1f, 1.5f); }
        this.timeBetweenActions = timeBetweenActions;
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(MovementHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        timeBetweenActions -= Time.deltaTime;
        if (!IsInRange())
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
            return;
        }
        else if (InAttackRange() && timeBetweenActions <= 0f)
        {
            timeBetweenActions = Random.Range(1.5f, 2.5f);
            stateMachine.SwitchState(new EnemyAttackingState(stateMachine));
            return;
        }
        else if (InAttackRange() && timeBetweenActions > 0f)
        {
            stateMachine.SwitchState(new EnemyEquippedState(stateMachine, timeBetweenActions));
            return;
        }
        MoveToPlayer(deltaTime);
        FacePlayer();
        stateMachine.Animator.SetFloat(SpeedHash, 1f, AnimatorDampTime, deltaTime);
    }

    public override void Exit()
    {
        stateMachine.Agent.ResetPath();
        stateMachine.Agent.velocity = Vector3.zero;
    }

}
