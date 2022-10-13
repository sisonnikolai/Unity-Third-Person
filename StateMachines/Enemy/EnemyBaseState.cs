using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(float deltaTime)
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        var movement = (motion + stateMachine.ForceReceiver.Movement);
        stateMachine.CharacterController.Move(movement * deltaTime);
    }

    protected bool IsInRange()
    {
        if (stateMachine.Player.IsDead) { return false; }
        float playerDistance = Vector3.Distance(stateMachine.Player.transform.position, stateMachine.transform.position);
        return playerDistance <= stateMachine.DetectionRange;
    }

    protected bool InAttackRange()
    {
        if (stateMachine.Player.IsDead) { return false; }
        float playerDistance = Vector3.Distance(stateMachine.Player.transform.position, stateMachine.transform.position);
        return playerDistance <= stateMachine.AttackRange;
    }

    protected void FacePlayer()
    {
        if (stateMachine.Player != null)
        {
            var lookAt = (stateMachine.Player.transform.position - stateMachine.transform.position);
            lookAt.y = 0f;
            stateMachine.transform.rotation = Quaternion.LookRotation(lookAt);
        }
    }

    // moved from EnemyChasingState
    protected void MoveToPlayer(float deltaTime)
    {
        if (stateMachine.Agent.isOnNavMesh)
        {
            stateMachine.Agent.destination = stateMachine.Player.transform.position;
            Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
        }
        stateMachine.Agent.velocity = stateMachine.CharacterController.velocity;
    }
    
    protected void ReturnToLocomotion()
    {
        if (stateMachine.WeaponHandler.IsWeaponEquipped())
        {
            stateMachine.SwitchState(new EnemyArmedIdleState(stateMachine, 0f));
        }
        else
        {
            stateMachine.SwitchState(new EnemyIdleState(stateMachine));
        }
    }
}
